using coding_test_ranking.infrastructure.persistence.models;
using System.Collections.Generic;

namespace coding_test_ranking.infrastructure.persistence.repositories
{
    public interface IAdsRepository
    {
        public void Save(AdVO ad);
        public IList<AdVO> GetAll();
        public IList<AdVO> GetBestByScore(int limit);
        public IList<AdVO> GetWorstByScore(int limit);
        AdVO GetById(int id);

        void Delete(AdVO value);
    }
}
