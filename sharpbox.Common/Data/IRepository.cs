using System.Collections.Generic;

namespace sharpbox.Common.Data
{
  public interface IRepository<T>
    {
        T Get(int key);

        IEnumerable<T> Get();
    }
}