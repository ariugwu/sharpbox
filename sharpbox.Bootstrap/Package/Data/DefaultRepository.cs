using System.Collections.Generic;
using sharpbox.Common.Data;

namespace sharpbox.WebLibrary.Data
{
    using System;

    public class DefaultRepository<T> : IRepository<T> where T : new()
    {
        public DefaultRepository(Io.Client file)
        {
            this.File = file;
            this._fileName = string.Format("{0}.dat", typeof(T).Name);
        }

        public Io.Client File { get; set; }

        private string _fileName;

        public T Get(int id)
        {
            if (this.File.Exists(this._fileName))
            {
                return this.File.Read<T>(this._fileName);
            }
            else
            {
                return new T();
            }
        }

        public IEnumerable<T> Get()
        {
            throw new NotImplementedException();
        }
    }
}