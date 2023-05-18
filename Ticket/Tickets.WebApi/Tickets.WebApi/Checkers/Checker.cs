using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Tickets.WebApi.Checkers
{
    public class Checker
    {
        private readonly HttpClient _httpClient;

        public Checker()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> AreIdsValidAsync(int journeyId, int passengerId)
        {
            bool isJourneyValid = await CheckIdAsync("journeys", journeyId);
            bool isPassengerValid = await CheckIdAsync("passengers", passengerId);

            return isJourneyValid && isPassengerValid;
        }

        private async Task<bool> CheckIdAsync(string service, int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:44308/api/"+service+"/"+id);

            return response.IsSuccessStatusCode;
        }
    }
}
