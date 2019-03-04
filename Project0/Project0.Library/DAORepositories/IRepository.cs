using System.Collections.Generic;

namespace Project0.Library.DAORepositories
{
    public interface IRepository<T>
    {
        void AddT(T obj);
        void UpdateT(T obj);
        void DeleteT(T obj);

        T GetTById(int id);
        IEnumerable<T> GetAllT();

    }


}
