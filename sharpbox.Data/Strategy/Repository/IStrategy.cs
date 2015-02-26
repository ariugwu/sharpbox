using System.Collections.Generic;

namespace sharpbox.Data.Strategy.Repository
{
    public interface IStrategy<T>
    {
        Dictionary<string, object> Props { get; set; } 
        List<T> Entities { get; set; }
        
        IEnumerable<T> All();
        T Create(T entity);
        T Get(int id);
        T Update(T entity);
        IEnumerable<T> UpdateAll(IEnumerable<T> list);
        void Delete(T entity);

    }
}
