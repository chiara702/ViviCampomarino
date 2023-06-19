using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ViviCampomarino.ServizioNavetta {
    public class Position {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime FixTime { get; set; }
        public double Altitude { get; set; }
        public double Speed { get; set; }
        public double Accurancy { get; set; }

    }

    public class TraccarApiClient {
        private const string Username = "dimariafabio@gmail.com";
        private const string Password = "123456";

        private HttpClient _httpClient;

        public TraccarApiClient() {
            _httpClient = new HttpClient();
        }

        public async Task<List<Position>> GetPositionsAsync() {
            // Configura l'autenticazione di base
            var authenticationHeaderValue = CreateAuthenticationHeaderValue(Username, Password);
            _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;

            var response = await _httpClient.GetAsync("https://demo.traccar.org/api/positions");
            if (response.IsSuccessStatusCode) {
                var responseContent = await response.Content.ReadAsStringAsync();
                var positions = JsonConvert.DeserializeObject<List<Position>>(responseContent);

                return positions;
            } else {
                throw new Exception($"Errore durante la richiesta dei dati di posizione: {response.StatusCode}");
            }
        }

        private AuthenticationHeaderValue CreateAuthenticationHeaderValue(string username, string password) {
            var encodedCredentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{username}:{password}"));
            return new AuthenticationHeaderValue("Basic", encodedCredentials);
        }
    }
}
