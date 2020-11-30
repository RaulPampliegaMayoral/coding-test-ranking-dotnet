using coding_test_ranking.infrastructure.persistence.models;
using System.Collections.Generic;
using System.Linq;

namespace coding_test_ranking.infrastructure.persistence.repositories
{
    public class AdsRepository : IAdsRepository
    {
        IRepository<AdVO> _repository;

        public AdsRepository(IRepository<AdVO> repository)
        {
            _repository = repository;
        }

        public void Delete(AdVO value)
        {
            _repository.Delete(value);
        }

        public IList<AdVO> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public IList<AdVO> GetBestByScore(int limit)
        {
            return _repository.GetAll().Where(c => c.Score >= limit).OrderByDescending(c => c.Score).ToList();
        }

        public AdVO GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IList<AdVO> GetWorstByScore(int limit)
        {
            return _repository.GetAll().Where(c => !c.Score.HasValue || c.Score < limit).OrderByDescending(c => c.Score).ToList();
        }

        public void Save(AdVO ad)
        {
            _repository.SaveOrUpdate(ad);
        }

    }
}
