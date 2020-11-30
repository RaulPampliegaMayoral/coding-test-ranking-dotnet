using System.Collections.Generic;

namespace coding_test_ranking.infrastructure.persistence.repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);

        void SaveOrUpdate(T value);
        void Delete(T value);
    }
}
