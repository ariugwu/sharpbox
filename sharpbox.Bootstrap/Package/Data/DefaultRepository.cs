using System.Collections.Generic;
using sharpbox.WebLibrary.Data;

namespace sharpbox.Bootstrap.Package.Data
{
  public class DefaultRepository<T> : IRepository<T> where T : new()
  {
    public T GetById(int id)
    {
      return new T();
    }

    public IEnumerable<T> GetAll()
    {
      return new List<T>();
    }
  }
}