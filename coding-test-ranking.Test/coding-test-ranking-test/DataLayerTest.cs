using coding_test_ranking.infrastructure.persistence.models;
using coding_test_ranking.infrastructure.persistence.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace coding_test_ranking_test
{
    public class DataLayerTest : IDisposable
    {
        private PicturesRepository _pictureRepository;
        private AdsRepository _adRepository;

        public DataLayerTest()
        {
            _pictureRepository = new PicturesRepository(new InMemoryPersistence<PictureVO>());
            _adRepository = new AdsRepository(new InMemoryPersistence<AdVO>());
        }

        public void Dispose()
        {
            _pictureRepository = null;
            _adRepository = null;
        }

        [Fact]
        public void GetAllPicturesTest()
        {
            var data = _pictureRepository.GetAll();

            Assert.True(data != null);
            Assert.True(data.Count() == 8);
        }
        
        [Fact]
        public void GetAllAdsTest()
        {
            var data = _adRepository.GetAll();

            Assert.True(data != null);
            Assert.True(data.Count() == 8);
        }

        [Fact]
        public void GetPictureByIdTest()
        {
            var data = _pictureRepository.GetById(4);

            Assert.True(data != null);
            Assert.True("http://www.idealista.com/pictures/4".Equals(data.Url));
        }

        [Fact]
        public void GetAdByIdTest()
        {
            var data = _adRepository.GetById(7);

            Assert.True(data != null);
            Assert.True("Garaje en el centro de Albacete".Equals(data.Description));
        }

        [Fact]
        public void AddAndRemoveTest()
        {
            var ad = new AdVO { Typology = "GARAGE", Description = "Garaje de prueba", Pictures = new List<int>(), HouseSize = 150 };

            _adRepository.Save(ad);

            var value = _adRepository.GetById(ad.Id);
            Assert.NotNull(value);
            Assert.True(value.Description == "Garaje de prueba");

            _adRepository.Delete(value);

            Assert.Null(_adRepository.GetById(ad.Id));
        }
    }
}
