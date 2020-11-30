using System.Collections.Generic;
using coding_test_ranking.infrastructure.api;
using coding_test_ranking.infrastructure.services;
using Microsoft.AspNetCore.Mvc;

namespace coding_test_ranking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private IAdsService _service;

        public AdsController(IAdsService service)
        {
            _service = service;
        }

        // GET api/values
        [HttpGet]
        [Route("values")]
        public ActionResult<IEnumerable<QualityAd>> qualityListing()
        {
            var data = _service.GetQualityAds();
            return data;
        }
        [HttpGet]
        [Route("public")]
        public ActionResult<IEnumerable<PublicAd>> publicListing()
        {
            var data = _service.GetPublicAds();
            return data;
        }

        [HttpGet]
        [Route("calculateScore")]
        public ActionResult calculateScore()
        {
            _service.calculateScoreForAds();
            return StatusCode(200);
        }
    }
}
