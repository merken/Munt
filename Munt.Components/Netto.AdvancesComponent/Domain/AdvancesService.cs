
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Netto.AdvancesComponent.Domain
{
    public class AdvancesService : IAdvancesService
    {
        private readonly HttpClient client;
        public AdvancesService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<List<Advance>> GetAdvancesForEmployeeId(int employeeId)
        {
            var response = await this.client.GetAsync($"/advances/{employeeId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Advances API did not respond with OK!");
                
            var content = await response.Content.ReadAsStringAsync();
        
            var advances = JsonConvert.DeserializeObject<List<Advance>>(content);

            return advances;
        }
    }
}