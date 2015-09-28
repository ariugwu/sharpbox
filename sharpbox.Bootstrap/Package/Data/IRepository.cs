namespace sharpbox.WebLibrary.Data
{
    using System.Collections.Generic;

    public interface IRepository<T>
    {
        T Get(int key);

        IEnumerable<T> Get();
    }
}