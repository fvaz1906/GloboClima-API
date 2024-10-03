using GloboClima.Domain.Entities;
using System.Net.Http.Json;

namespace GloboClima.Infra.Data.Api
{
    public class CountryApiService
    {
        private readonly HttpClient _httpClient;

        public CountryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CountryData[]?> GetCountryAsync(string countryName)
        {
            try
            {
                var url = $"https://restcountries.com/v3.1/name/{countryName}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var countryData = await response.Content.ReadFromJsonAsync<CountryData[]>();
                return countryData;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
                return null;
            }
        }
    }
}
