using DockerWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using SharedComp;
using System.Net.Http.Formatting;

namespace DockerWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration config;

        public HomeController(ILogger<HomeController> logger, IConfiguration _config)
        {
            _logger = logger;
            this.config = _config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> WeatherApi()
        {
            IEnumerable<WeatherForecast> items = null;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(config["WebAPiUrl"]);
            var response = client.GetAsync("WeatherForecast");
            response.Wait();
            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IEnumerable<WeatherForecast>>();
                readTask.Wait();
                items = readTask.Result;
            }
            return View(items);
        }



    }
}
