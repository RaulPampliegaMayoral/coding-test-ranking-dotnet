using coding_test_ranking.infrastructure.persistence.models;
using coding_test_ranking.infrastructure.persistence.repositories;
using System.Linq;

namespace coding_test_ranking.infrastructure.services
{
    public class PicturesService : IPicturesService
    {
        private readonly IPicturesRepository _repository;

        public PicturesService(IPicturesRepository repository)
        {
            _repository = repository;
        }

        public PictureVO GetOrSavePicture(string url, string quality)
        {
            if (url == null)
                return null;

            var picture = _repository.GetAll().FirstOrDefault(c => c.Url.Equals(url));
            if (picture != null)
                return picture;

            if (quality == null)
                quality = "SD";

            picture = new PictureVO
            {
                Url = url,
                Quality = quality
            };
            _repository.Save(picture);

            return picture;
        }
    }
}
