using coding_test_ranking.infrastructure.persistence.models;
using coding_test_ranking.infrastructure.persistence.repositories;
using coding_test_ranking.infrastructure.services;
using coding_test_ranking.infrastructure.services.Calculator;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace coding_test_ranking_test
{
    public class AdsServiceTest : IDisposable
    {
        private PicturesRepository _pictureRepository;
        private AdsRepository _adRepository;
        private IRuleCalculator _calculator;
        private AdsService _service;

        public AdsServiceTest()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _pictureRepository = new PicturesRepository(new InMemoryPersistence<PictureVO>());
            _adRepository = new AdsRepository(new InMemoryPersistence<AdVO>());
            _calculator = new RuleCalculator(_pictureRepository);
            _service = new AdsService(_adRepository, _pictureRepository, _calculator, configuration);
        }

        public void Dispose()
        {
            _adRepository = null;
            _pictureRepository = null;
            _service = null;
        }

        [Fact]
        public void NoQualityAdsTest()
        {
            var data = _service.GetQualityAds();
            Assert.NotNull(data);
            Assert.True(data.Count == 8);
        }

        [Fact]
        public void OnePublicAdsTest()
        {
            var ad = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            _adRepository.Save(ad);

            var data = _service.GetPublicAds();
            Assert.NotNull(data);
            Assert.True(data.Count == 1);
            Assert.True(data[0].Description == "Garaje de prueba");

            _adRepository.Delete(ad);
        }

        [Fact]
        public void NoPublicAdsOneQualityAdTest()
        {
            var ad = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            _adRepository.Save(ad);

            var dataPublic = _service.GetPublicAds();
            Assert.NotNull(dataPublic);
            Assert.True(dataPublic.Count == 1);
            Assert.True(dataPublic[0].Description == "Garaje de prueba");

            var dataQuality = _service.GetQualityAds();
            Assert.NotNull(dataQuality);
            Assert.True(dataQuality.Count == 9);

            _adRepository.Delete(ad);
        }

        [Fact]
        public void OnePublicAdsOneQualityAdTest()
        {
            var ad1 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            var ad2 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba 2", Pictures = new List<int>(), HouseSize = 150, Score = 39, IrrelevantSince = DateTime.Now };
            _adRepository.Save(ad1);
            _adRepository.Save(ad2);

            var dataPublic = _service.GetPublicAds();
            Assert.NotNull(dataPublic);
            Assert.True(dataPublic.Count == 1);
            Assert.True(dataPublic[0].Description == "Garaje de prueba");

            var dataQuality = _service.GetQualityAds();
            Assert.NotNull(dataQuality);
            Assert.True(dataQuality.Count == 10);
            Assert.True(dataQuality.First(c => c.Id == ad2.Id).Description == "Garaje de prueba 2");

            _adRepository.Delete(ad1);
            _adRepository.Delete(ad2);
        }

        [Fact]
        public void DescriptionRuleTest()
        {
            var rule = new DescriptionRule();

            var ad1 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad1) == 5);

            var ad2 = new AdVO { Typology = "GARAGE", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad2) == 0);

            var ad3 = new AdVO { Typology = "GARAGE", Description = "", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad3) == 0);

            var ad4 = new AdVO { Typology = "GARAGE", Description = "                 ", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad4) == 0);
        }

        [Fact]
        public void PictureRuleTest()
        {
            var rule = new PicturesRule(_pictureRepository);

            var ad1 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad1) == -10);

            var ad2 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>() { 1 }, HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad2) == 10);

            var ad3 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>() { 1, 2 }, HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad3) == 30);

            var ad4 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>() { 4, 2 }, HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad4) == 40);

            var ad5 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = null, HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad5) == -10);
        }

        [Fact]
        public void DescriptionSizeRuleTest()
        {
            var rule = new DescriptionSizeRule();

            var ad1 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad1) == 0);

            var ad2 = new AdVO { Typology = "FLAT", Description = "Descripcion de prueba Descripcion", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad2) == 10);

            var ad3 = new AdVO { Typology = "CHALET", Description = "Descripcion de prueba Descripcion", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad3) == 0);

            var ad4 = new AdVO { Typology = "CHALET", Description = "Descripcion de prueba Descripcion Descripcion de ", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad4) == 0);

            var ad5 = new AdVO { Typology = "FLAT", Description = "Descripcion de prueba Descripcion Descripcion de ", Pictures = new List<int>() { 1 }, HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad5) == 10);

            var ad6 = new AdVO { Typology = "CHALET", Description = "Descripcion de prueba Descripcion Descripcion de 1", Pictures = new List<int>() { 1 }, HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad6) == 0);

            var ad7 = new AdVO { Typology = "FLAT", Description = "Descripcion de prueba Descripcion Descripcion de 1", Pictures = new List<int>() { 1 }, HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad7) == 30);

            var ad8 = new AdVO { Typology = "CHALET", Description = "Descripcion de prueba Descripcion Descripcion de 1 ", Pictures = new List<int>() { 1 }, HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad8) == 20);
        }

        [Fact]
        public void WordsAppearanceTest()
        {
            var rule = new WordAppearanceRule();

            var ad1 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad1) == 0);

            var ad2 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba siendo luminoso", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad2) == 5);

            var ad3 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba siendo luminoso y nuevo y otra vez luminoso", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad3) == 10);

            var ad4 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba céntrico siendo luminoso y nuevo", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad4) == 15);

            var ad5 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba centrico siendo luminoso y nuevo", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad5) == 10);

            var ad6 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba céntrico siendo luminoso y nuevo pero no reformado", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad6) == 20);

            var ad7 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba céntrico siendo luminoso y nuevo pero no reformado en el ático del garaje", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad7) == 25);

            var ad8 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba céntrico siendo luminoso y nuevo pero no reformado en el atico del garaje", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad8) == 20);
        }

        [Fact]
        public void AdCompleteTest()
        {
            var rule = new AdCompleteRule();

            var ad1 = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>(), HouseSize = 150, Score = 40 };
            Assert.True(rule.Calculate(ad1) == 0);

            var ad2 = new AdVO { Typology = "GARAGE", Pictures = new List<int>() { 1 } };
            Assert.True(rule.Calculate(ad2) == 40);

            var ad3 = new AdVO { Typology = "FLAT", Description = "Descripcion de prueba", Pictures = new List<int> { 1 }, HouseSize = 100 };
            Assert.True(rule.Calculate(ad3) == 40);

            var ad4 = new AdVO { Typology = "FLAT", Description = "Descripcion de prueba", Pictures = new List<int> { 1 } };
            Assert.True(rule.Calculate(ad4) == 0);

            var ad5 = new AdVO { Typology = "CHALET", Description = "Descripcion de prueba", Pictures = new List<int>(), HouseSize = 100 };
            Assert.True(rule.Calculate(ad5) == 0);

            var ad6 = new AdVO { Typology = "CHALET", Description = "Descripcion de prueba", Pictures = new List<int> { 1 }, HouseSize = 100, GardenSize = 50 };
            Assert.True(rule.Calculate(ad6) == 40);

            var ad7 = new AdVO { Typology = "CHALET", Description = "Descripcion de prueba", HouseSize = 100, GardenSize = 50 };
            Assert.True(rule.Calculate(ad7) == 0);
        }

        [Fact]
        public void RuleCalculatorTest()
        {
            var calculator = new RuleCalculator(_pictureRepository);

            var ad = new AdVO { Typology = "FLAT", Description = "Descripcion de prue", Pictures = new List<int> { 1 }, HouseSize = 100 };
            var score = calculator.CalculateScore(ad);
            Assert.True( score == 55);

            var ad1 = new AdVO { Typology = "FLAT", Description = "Descripcion de prueba", Pictures = new List<int> { 1 }, HouseSize = 100 };
            Assert.True(calculator.CalculateScore(ad1) == 65);

            var ad2 = new AdVO { Typology = "FLAT", Description = "Descripcion de prueba céntrico siendo luminoso y nuevo pero no reformado en el ático del garaje", Pictures = new List<int> { 1 }, HouseSize = 100 };
            Assert.True(calculator.CalculateScore(ad2) == 110);

            var ad3 = new AdVO { Typology = "FLAT", Description = "Descripcion de prueba céntrico siendo luminoso y nuevo pero no reformado en el ático del garaje", HouseSize = 100 };
            score = calculator.CalculateScore(ad3);
            Assert.True(score == 50);
        }
    }
}
