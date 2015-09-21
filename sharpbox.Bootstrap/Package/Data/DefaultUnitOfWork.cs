using System;

namespace sharpbox.WebLibrary.Data
{
    public class DefaultUnitOfWork<T> : IUnitOfWork<T>
    {
        public T Insert(T instance)
        {
            throw new NotImplementedException();
        }

        public T Update(T instance)
        {
            throw new NotImplementedException();
        }

        public T Delete(T instance)
        {
            throw new NotImplementedException();
        }
    }
}
