using System.Collections.Generic;

namespace sharpbox.Data.Strategy
{
    public interface IStrategy<T>
    {
        IEnumerable<T> All();
        T Create(T entity);
        T Get(int id);
        T Update(T entity);
        void Delete(T entity);
    }
}
