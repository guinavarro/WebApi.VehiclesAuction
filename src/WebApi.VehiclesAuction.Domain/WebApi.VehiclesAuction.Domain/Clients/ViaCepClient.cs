using System.Threading;
using WebApi.VehiclesAuction.Domain.Interfaces.Clients;

namespace WebApi.VehiclesAuction.Domain.Clients
{
    public class ViaCepClient : IViaCepClient
    {
        private const string _baseUrl = "https://viacep.com.br";
        private readonly HttpClient _httpClient;

        public ViaCepClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<ViaCepResult> FindByZip(string zipCode)
        {
            using (var httpResponse = await _httpClient.GetAsync($"/ws/{zipCode}/json").ConfigureAwait(false))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return ViaCepResult.FromJson(json);
                }

                return null;
            }
        }

     
    }
}
