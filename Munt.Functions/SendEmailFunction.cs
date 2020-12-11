using System;
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
            msg.AddTos(recipients);
            msg.SetSubject($"Payslip for {emailMessage.Employee} {emailMessage.Result}");
            msg.AddContent(MimeType.Html,
                $"<pre id=\"json\">{emailMessage.Journey}</pre><script type=\"text/javascript\">document.getElementById(\"json\").innerHTML = JSON.stringify(data, undefined, 2);</script>");
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