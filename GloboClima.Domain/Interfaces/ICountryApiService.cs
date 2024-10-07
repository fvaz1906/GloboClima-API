using GloboClima.Domain.Entities;

namespace GloboClima.Domain.Interfaces
{
    public interface ICountryApiService
    {
        Task<CountryData[]?> GetCountryAsync(string countryName);
    }
}
