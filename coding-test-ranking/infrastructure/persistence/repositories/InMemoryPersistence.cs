using coding_test_ranking.infrastructure.persistence.models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace coding_test_ranking.infrastructure.persistence.repositories
{
    public class InMemoryPersistence<T> : IRepository<T> where T : Entity
    {
        private static List<AdVO> ads;
        private static List<PictureVO> pictures;

        private static int nextId = 9;

        public InMemoryPersistence()
        {
            ads = new List<AdVO>
            {
                new AdVO { Id = 1, Typology = "CHALET", Description = "Este piso es una ganga, compra, compra, COMPRA!!!!!", Pictures = new List<int>(), HouseSize = 300 },
                new AdVO { Id = 2, Typology = "FLAT", Description = "Nuevo ático céntrico recién reformado. No deje pasar la oportunidad y adquiera este ático de lujo", Pictures = new List<int> { 4 }, HouseSize = 300 },
                new AdVO { Id = 3, Typology = "CHALET", Description = "", Pictures = new List<int> { 2 }, HouseSize = 300 },
                new AdVO { Id = 4, Typology = "FLAT", Description = "Ático céntrico muy luminoso y recién reformado, parece nuevo", Pictures = new List<int> { 5 }, HouseSize = 300 },
                new AdVO { Id = 5, Typology = "FLAT", Description = "Pisazo,", Pictures = new List<int> { 3, 8 }, HouseSize = 300 },
                new AdVO { Id = 6, Typology = "GARAGE", Description = "", Pictures = new List<int> { 6 }, HouseSize = 300 },
                new AdVO { Id = 7, Typology = "GARAGE", Description = "Garaje en el centro de Albacete", Pictures = new List<int>(), HouseSize = 300 },
                new AdVO { Id = 8, Typology = "CHALET", Description = "Maravilloso chalet situado en lAs afueras de un pequeño pueblo rural. El entorno es espectacular, las vistas magníficas. ¡Cómprelo ahora!", Pictures = new List<int> { 1, 7 }, HouseSize = 300 }
            };

            pictures = new List<PictureVO>
            {
                new PictureVO { Id = 1, Url = "http://www.idealista.com/pictures/1", Quality = "SD" },
                new PictureVO { Id = 2, Url = "http://www.idealista.com/pictures/2", Quality = "HD" },
                new PictureVO { Id = 3, Url = "http://www.idealista.com/pictures/3", Quality = "SD" },
                new PictureVO { Id = 4, Url = "http://www.idealista.com/pictures/4", Quality = "HD" },
                new PictureVO { Id = 5, Url = "http://www.idealista.com/pictures/5", Quality = "SD" },
                new PictureVO { Id = 6, Url = "http://www.idealista.com/pictures/6", Quality = "SD" },
                new PictureVO { Id = 7, Url = "http://www.idealista.com/pictures/7", Quality = "SD" },
                new PictureVO { Id = 8, Url = "http://www.idealista.com/pictures/8", Quality = "HD" }
            };
        }

        public void Delete(T value)
        {
            if (value == null)
                return;

            var valueFromList = GetById(value.Id);
            if (valueFromList == null)
                return;

            lock(this)
            {
                if (value.GetType() == typeof(AdVO))
                {
                    ads.RemoveAll(c => c.Id == value.Id);
                }

                if (value.GetType() == typeof(PictureVO))
                {
                    pictures.RemoveAll(c => c.Id == value.Id);
                }
            }
        }
        public void SaveOrUpdate(T value)
        {
            if (value == null)
                return;

            if (value.Id <= 0)
                value.Id = nextId++;
            else
                Delete(value);

            SaveValue(value);

        }

        private void SaveValue(T value)
        {
            lock (this)
            {
                if (value.GetType() == typeof(AdVO))
                {
                    ads.Add((AdVO)Convert.ChangeType(value, typeof(AdVO)));
                }

                if (value.GetType() == typeof(PictureVO))
                {
                    pictures.Add((PictureVO)Convert.ChangeType(value, typeof(PictureVO)));
                }
            }
        }

        public IEnumerable<T> GetAll()
        {
            var repositoryType = typeof(T);
            if (repositoryType == typeof(AdVO))
                return (IEnumerable<T>)ads.Select(c => c.Clone()).ToList();

            if (repositoryType == typeof(PictureVO))
                return (IEnumerable<T>)pictures.Select(c => c.Clone()).ToList();

            return null;
        }

        public T GetById(int id)
        {
            var values = GetAll();
            if (values == null)
                return default;
            
            return values.FirstOrDefault(c => c.Id == id);
        }
    }
}
