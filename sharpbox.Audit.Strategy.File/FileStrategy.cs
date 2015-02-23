using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy.File
{
    public class FileStrategy : IStrategy
    {
        private Io.Client _file;
        private Dictionary<string, object> _props;

        public FileStrategy(Dispatch.Client dispatcher, Io.Strategy.IStrategy persistenceStrategy,
            Dictionary<string, object> props)
        {
            _file = new Io.Client(persistenceStrategy);
            _props = props;

            var filePath = _props["filePath"].ToString();
            if (!_file.Exists(filePath)) _file.Write(filePath, new List<Response>());
            Trail = _file.Read<List<Response>>(filePath);
        }

        public List<Response> Trail { get; set; }

        public void Record(Response response)
        {
            Trail.Add(response);
            _file.Write(_props["filePath"].ToString(), Trail);
        }

    }
}
