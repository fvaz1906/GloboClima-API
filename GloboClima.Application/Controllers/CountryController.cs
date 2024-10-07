﻿using GloboClima.Infra.Data.Api;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly CountryApiService _countryApiService;

        public CountryController(CountryApiService countryApiService)
        {
            _countryApiService = countryApiService;
        }

        /// <summary>
        /// Obtém o País encontrado.
        /// </summary>
        /// <returns>um país encontrado.</returns>
        [HttpGet("{countryName}")]
        public async Task<IActionResult> GetCountry(string countryName)
        {
            var countryData = await _countryApiService.GetCountryAsync(countryName);

            if (countryData != null && countryData.Length > 0)
            {
                return Ok(countryData[0]);
            }

            return NotFound("Informações do país não encontradas.");
        }
    }
}
