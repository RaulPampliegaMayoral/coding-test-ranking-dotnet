using coding_test_ranking.infrastructure.persistence.models;
using System.Collections.Generic;
using System.Linq;

namespace coding_test_ranking.infrastructure.persistence.repositories
{
    public class PicturesRepository : IPicturesRepository
    {
        private IRepository<PictureVO> _repository;

        public PicturesRepository(IRepository<PictureVO> repository)
        {
            _repository = repository;
        }

        public void Delete(PictureVO value)
        {
            _repository.Delete(value);
        }

        public IEnumerable<PictureVO> GetAll()
        {
            return _repository.GetAll();
        }

        public PictureVO GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IList<PictureVO> GetPictures(IEnumerable<int> ids)
        {
            if (ids == null)
                return new List<PictureVO>();

            return _repository.GetAll().Where(c => ids.Contains(c.Id)).ToList();
        }

        public IList<PictureVO> GetPictures(IEnumerable<string> urls)
        {
            if (urls == null)
                return new List<PictureVO>();

            return _repository.GetAll().Where(c => urls.Contains(c.Url)).ToList();
        }

        public void Save(PictureVO value)
        {
            _repository.SaveOrUpdate(value);
        }
    }
}
