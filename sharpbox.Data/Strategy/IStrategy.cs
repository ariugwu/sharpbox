using System.Collections.Generic;

namespace sharpbox.Data.Strategy
{
    public interface IStrategy<T>
    {
        Dictionary<string, object> AuxInfo { get; set; } 
        List<T> Entities { get; set; }
        
        IEnumerable<T> All(Dispatch.Client dispatcher);
        T Create(Dispatch.Client dispatcher, T entity);
        T Get(Dispatch.Client dispatcher, int id);
        T Update(Dispatch.Client dispatcher, T entity);
        IEnumerable<T> UpdateAll(Dispatch.Client dispatcher, IEnumerable<T> list);
        void Delete(Dispatch.Client dispatcher, T entity);

    }
}
