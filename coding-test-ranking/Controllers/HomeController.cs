using coding_test_ranking.infrastructure.api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace coding_test_ranking.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _apiBaseUrl;
        private readonly static HttpClient _apiClient = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        });

        public HomeController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration.GetValue<string>("WebApiBaseUrl");
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("publicAds")]
        public async Task<IActionResult> PublicAds()
        {
            var publicAds = new List<PublicAd>();
            using(var response = await _apiClient.GetAsync($"{_apiBaseUrl}/public"))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    publicAds.AddRange(JsonConvert.DeserializeObject<IEnumerable<PublicAd>>(apiResponse));
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "An error ocurred getting public ads listing");
                }
            }
            return View(publicAds);
        }

        [Route("qualityAds")]
        public async Task<IActionResult> QualityAds()
        {
            var qualityAds = new List<QualityAd>();
            using (var response = await _apiClient.GetAsync($"{_apiBaseUrl}/values"))
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    qualityAds.AddRange(JsonConvert.DeserializeObject<IEnumerable<QualityAd>>(apiResponse));
                }
                else
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "An error ocurred getting quality ads listing");
                }
            }
            return View(qualityAds);
        }

        [Route("calculateScore")]
        public async Task<IActionResult> CalculateScore()
        {
            using (var response = await _apiClient.GetAsync($"{_apiBaseUrl}/calculateScore"))
            {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "An error ocurred calculating scores for ads");
                }
            }

            return View();
        }
    }
}
