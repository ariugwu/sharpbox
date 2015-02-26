using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Data.Strategy.Repository;

namespace sharpbox.Data.Strategy.File
{
    public class FileStrategy<T> : IStrategy<T>
    {
        /// <summary>
        /// The this strategy expects a fully configured Io.Client included in the props named "ioClient" of type IEnummerable-T and "filePath" to say where to read/write from
        /// </summary>
        /// <param name="props"></param>
        public FileStrategy(Dictionary<string, object> props)
        {
            Props = props;
            _io = (Io.Client) props["ioClient"];
            _filePath = (string) props["filePath"];
        }

        private Io.Client _io;
        private string _filePath;
        private List<T> _entities;


        public Dictionary<string, object> Props { get; set; }

        public List<T> Entities
        {
            get { return _entities ?? (_entities = _io.Read<List<T>>(_filePath)); }
            set { _entities = value; }
        }

        public IEnumerable<T> All()
        {
            return Entities;
        }

        public T Create(T entity)
        {
            Entities.Add(entity);
            _io.Write(_filePath, Entities);
            return entity;;
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> UpdateAll(IEnumerable<T> list)
        {
            Entities = list.ToList();
            _io.Write(_filePath, Entities);

            return Entities;
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
