using System;

namespace sharpbox.WebLibrary.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using sharpbox.Io.Model;

    public class DefaultUnitOfWork<T> : IUnitOfWork<T>
        where T : new()
    {
        private string _fileName { get; set; }

        public DefaultUnitOfWork(Io.Client file)
        {
            this.File = file;
            this._fileName = string.Format("{0}.dat",typeof(T).Name);
        } 

        public Io.Client File { get; set; }

        public T Insert(T instance)
        {
            this.File.Write(this._fileName, instance);

            return instance;
        }

        public T Update(T instance)
        {
            return Insert(instance);
        }

        public T Delete(T instance)
        {
            throw new NotImplementedException();
        }
    }
}
