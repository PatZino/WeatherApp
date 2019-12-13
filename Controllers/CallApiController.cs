using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;
// Reference Newtonsoft
using Newtonsoft.Json;

namespace MyMvcApp.Controllers
{
    public class CallApiController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CallApiController(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        private async Task<WeatherForecast> GetWeatherForecasts()
        {
            // Get an instance of HttpClient from the factpry that we registered
            // in Startup.cs
            var client = _httpClientFactory.CreateClient("API Client");

            // Call the API & wait for response. 
            // If the API call fails, call it again according to the re-try policy
            // specified in Startup.cs
            var result = await client.GetAsync("/api/location/1103816/");

            if (result.IsSuccessStatusCode)
            {
                // Read all of the response and deserialise it into an instace of
                // WeatherForecast class
                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherForecast>(content);
            }
            return null;
        }

        public async Task<IActionResult> View()
        {
            var model = await GetWeatherForecasts();
            // Pass the data into the View
            return View(model);
        }

    }
}