using System.Collections.Generic;

namespace sharpbox.Data.Strategy
{
    public interface IStrategy<T>
    {
        Dictionary<string, object> AuxInfo { get; set; } 
        List<T> Entities { get; set; }

        void Init();
        
        IEnumerable<T> All();
        T Create(T entity);
        T Get(int id);
        T Update(T entity);
        IEnumerable<T> UpdateAll(IEnumerable<T> list); 
        void Delete(T entity);

    }
}
