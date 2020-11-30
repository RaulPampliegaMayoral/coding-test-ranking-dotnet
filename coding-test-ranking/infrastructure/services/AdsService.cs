using coding_test_ranking.infrastructure.api;
using coding_test_ranking.infrastructure.persistence.models;
using coding_test_ranking.infrastructure.persistence.repositories;
using coding_test_ranking.infrastructure.services.Calculator;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace coding_test_ranking.infrastructure.services
{
    public class AdsService : IAdsService
    {
        private readonly IAdsRepository _adsRepository;
        private readonly IPicturesRepository _picturesRepository;
        private readonly IRuleCalculator _calculator;
        private readonly int limitToBeConsideredIrrelevant;

        public AdsService(IAdsRepository adsRepository, 
                          IPicturesRepository picturesRepository, 
                          IRuleCalculator calculator, 
                          IConfiguration configurator)
        {
            _adsRepository = adsRepository;
            _picturesRepository = picturesRepository;
            _calculator = calculator;
            limitToBeConsideredIrrelevant = configurator.GetValue<int>("LimitToBeConsideredIrrelevant");
        }

        public void calculateScoreForAds()
        {
            var allAds = _adsRepository.GetAll();
            foreach(var ad in allAds)
            {
                var score = _calculator.CalculateScore(ad);
                ad.IrrelevantSince = GetDateToBeConsideredIrrelevantIfNeeded(ad.Score, score);
                ad.Score = score;
                _adsRepository.Save(ad);
            }
        }

        private DateTime? GetDateToBeConsideredIrrelevantIfNeeded(int? originalScore, int score)
        {
            if (score < limitToBeConsideredIrrelevant && (!originalScore.HasValue || originalScore >= limitToBeConsideredIrrelevant))
                    return DateTime.Now;

            return null;
        }

        public List<QualityAd> GetQualityAds()
        {
            var ads = _adsRepository.GetAll().OrderByDescending(c => c.Score);
            var qualityAds = ads.Select(c => TransformAdToQualityAd(c)).ToList();
            return qualityAds;
        }

        public List<PublicAd> GetPublicAds()
        {
            var ads = _adsRepository.GetBestByScore(limitToBeConsideredIrrelevant);
            var publicAds = ads.Select(c => TransformAdToPublicAd(c)).ToList();
            return publicAds;
        }

        private QualityAd TransformAdToQualityAd(AdVO ad)
        {
            var qualityAd = new QualityAd
            {
                Id = ad.Id,
                Typology = ad.Description,
                Description = ad.Description,
                PictureUrls = _picturesRepository.GetPictures(ad.Pictures).Select(c => c.Url).ToList(),
                HouseSize = ad.HouseSize ?? 0,
                GardenSize = ad.GardenSize ?? 0,
                Score = ad.Score ?? 0,
            };
            
            if (ad.IrrelevantSince.HasValue)
                qualityAd.IrrelevantSince = ad.IrrelevantSince.Value;

            return qualityAd;
        }

        private AdVO TransformPublicADToAdVO(PublicAd publicAd)
        {
            return new AdVO
            {
                Id = publicAd.Id,
                Typology = publicAd.Typology,
                Description = publicAd.Description,
                HouseSize = publicAd.HouseSize,
                GardenSize = publicAd.GardenSize,
                Pictures = _picturesRepository.GetPictures(publicAd.PictureUrls).Select(c => c.Id).ToList(),
            };
        }

        private PublicAd TransformAdToPublicAd(AdVO ad)
        {
            return new PublicAd
            {
                Id = ad.Id,
                Typology = ad.Typology,
                Description = ad.Description,
                PictureUrls = _picturesRepository.GetPictures(ad.Pictures).Where(c => c.Url != null).Select(c => c.Url).ToList(),
                HouseSize = ad.HouseSize ?? 0,
                GardenSize = ad.GardenSize ?? 0,
            };
        }

        public List<PublicAd> GetAllAds()
        {
            var ads = _adsRepository.GetAll();
            var publicAds = ads.Select(c => TransformAdToPublicAd(c)).ToList();
            return publicAds;
        }

        public PublicAd GetAdById(int id)
        {
            var ad = _adsRepository.GetById(id);
            return TransformAdToPublicAd(ad);
        }

        public void DeleteAdById(int id)
        {
            var ad = _adsRepository.GetById(id);
            _adsRepository.Delete(ad);
        }

        public void SaveAd(PublicAd publicAd)
        {
            var ad = TransformPublicADToAdVO(publicAd);
            _adsRepository.Save(ad);
        }
    }
}
