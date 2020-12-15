using System;
using System.Linq;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Munt.Contract;
using Prise.Infrastructure;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Mail;
using Munt.Functions.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace Munt.Functions
{
    public class SendEmailFunction
    {
        private static EmailMessage Deserialize(string message)
        {
            return JsonConvert.DeserializeObject<EmailMessage>(message);
        }

        [FunctionName("SendEmailFunction")]
        public static async Task Run(
            [QueueTrigger("munt-email-queue", Connection = "AzureWebJobsStorage")]
            string message,
            ILogger log)
        {
            var emailMessage = Deserialize(message);
            var apiKey = GetEnvironmentVariable("SendGridAPIKey");
            var fromAddress = GetEnvironmentVariable("SendGridFromAddress");
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(fromAddress, "Munt Calc Function"));
            var recipients = new List<EmailAddress>
            {
                new EmailAddress(emailMessage.EmailAddress)
            };

            var result = emailMessage.CalculationResults.Sum(c=>c.Value);
            var period = $"{emailMessage.StartDate.ToShortDateString()} - {emailMessage.EndDate.AddDays(-1).ToShortDateString()}";
            msg.AddTos(recipients);
            msg.SetSubject($"Payslip for {emailMessage.Employee} for {period}");

            var rowBuilder= new StringBuilder();
            foreach(var row in emailMessage.CalculationResults.Select(c=> $"<tr><td>{c.Code}</td><td>{c.Description}</td><td>{c.Value}</td></tr>"))
                rowBuilder.AppendLine(row);
            var resultLine =  $"<tr><td colspan='3'>Total:</td><td>{result}</td></tr>";
            msg.AddContent(MimeType.Html,
                $@"<html><body><h3>Payslip for {emailMessage.Employee}</h3><div><table><tr><th>Code</th><th>Description</th><th>Value</th></tr>{rowBuilder}{resultLine}</table></div><div>Booked onto your bankaccount: {result}</div></body></html>");
            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode != HttpStatusCode.Accepted && response.StatusCode != HttpStatusCode.OK)
            {
                var body = await response.Body.ReadAsStringAsync();
                throw new Exception("Mail could not be sent: " + body);
            }
        }

        public static string GetEnvironmentVariable(string name)
        {
            return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}