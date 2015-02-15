using System.Collections.Generic;

namespace sharpbox.Data.Strategy
{
    public interface IStrategy<T>
    {
        Dictionary<string, object> AuxInfo { get; set; } 
        IEnumerable<T> Entities { get; set; }

        void Init();
        
        IEnumerable<T> All();
        T Create(T entity);
        T Get(int id);
        T Update(T entity);
        void Delete(T entity);

    }
}
