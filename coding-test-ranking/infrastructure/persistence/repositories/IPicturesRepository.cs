using coding_test_ranking.infrastructure.persistence.models;
using System.Collections.Generic;

namespace coding_test_ranking.infrastructure.persistence.repositories
{
    public interface IPicturesRepository
    {
        IList<PictureVO> GetPictures(IEnumerable<int> ids);
        IEnumerable<PictureVO> GetAll();
        PictureVO GetById(int id);

        void Save(PictureVO value);
        void Delete(PictureVO value);
    }
}
