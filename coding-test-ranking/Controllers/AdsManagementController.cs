using coding_test_ranking.infrastructure.api;
using coding_test_ranking.infrastructure.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace coding_test_ranking.Controllers
{
    public class AdsManagementController : Controller
    {
        private readonly IAdsService _service;
        private readonly IPicturesService _picturesService;

        public AdsManagementController(IAdsService service, IPicturesService pictureService)
        {
            _service = service;
            _picturesService = pictureService;
        }

        // GET: AdsManagement
        public ActionResult Index()
        {
            var ads = _service.GetAllAds();
            return View(ads);
        }

        // GET: AdsManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdsManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var typologyParam = collection["typology"].ToString();
                var descriptionParam = collection["description"].ToString();
                var sizeParam = int.Parse(collection["Housesize"].ToString());
                var gardenParam = int.Parse(collection["GardenSize"].ToString());

                var urlsParams = collection["picture_"].ToArray();
                var qualityParams = collection["quality_"].ToArray();

                var picturesParams = new List<string>();
                for(int i=0; i < urlsParams.Length; i++)
                {
                    var url = urlsParams[i];
                    var quality = qualityParams[i];
                    _picturesService.GetOrSavePicture(url, quality);

                    picturesParams.Add(url);
                }

                var ad1 = new PublicAd()
                {
                    Typology = typologyParam,
                    Description = descriptionParam,
                    HouseSize = sizeParam,
                    GardenSize = gardenParam,
                    PictureUrls = picturesParams,
                };
                _service.SaveAd(ad1);
            }
            catch
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "An error ocurred saving ad. Check data inserted");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AdsManagement/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                _service.DeleteAdById(id);
            }
            catch
            {
                ModelState.Clear();
                ModelState.AddModelError(string.Empty, "An error ocurred deleting ad");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
