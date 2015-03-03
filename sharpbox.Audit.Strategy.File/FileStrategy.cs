using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy.File
{
    public class FileStrategy : IStrategy
    {
        private Io.Client _file;
        private string _filePath;

        public FileStrategy(Io.Strategy.IStrategy persistenceStrategy, string filePath)
        {
            _file = new Io.Client(persistenceStrategy);
            _filePath = filePath;

            if (!_file.Exists(_filePath)) _file.Write(filePath, new List<Response>());
            Trail = _file.Read<List<Response>>(filePath);
        }

        public List<Response> Trail { get; set; }

        public void Record(Response response)
        {
            Trail.Add(response);
            _file.Write(_filePath, Trail);
        }

    }
}
