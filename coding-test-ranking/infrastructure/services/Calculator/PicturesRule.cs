using coding_test_ranking.infrastructure.persistence.models;
using coding_test_ranking.infrastructure.persistence.repositories;
using System.Linq;

namespace coding_test_ranking.infrastructure.services.Calculator
{
    public class PicturesRule : IRule
    {
        private readonly IPicturesRepository _picturesRepository;

        public PicturesRule(IPicturesRepository picturesRepository)
        {
            _picturesRepository = picturesRepository;
        }

        private const string HD = "HD";
        private const string SD = "SD";

        public int Calculate(AdVO ad)
        {
            if (ad.Pictures == null || ad.Pictures.Count() == 0)
                return -10;

            var score = 0;
            var pictures = _picturesRepository.GetPictures(ad.Pictures);
            score += 20 * pictures.Count(c => c.Quality.Equals(HD));
            score += 10 * pictures.Count(c => c.Quality.Equals(SD));

            return score;
        }
    }
}
