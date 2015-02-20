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
            if (!_file.Exists(filePath)) _file.Write(dispatcher, filePath, new List<Package>());
            Trail = _file.Read<List<Package>>(dispatcher, filePath);
        }

        public List<Package> Trail { get; set; }

        public void RecordDispatch(Dispatch.Client dispatcher, Package package)
        {
            Trail.Add(package);
            _file.Write(dispatcher, _props["filePath"].ToString(), Trail);
        }

        public void RecordDispatch(Dispatch.Client dispatcher, Request request)
        {
            var package = new Package
            {
                Entity = request.Entity,
                EventName = EventNames.OnRecordAction,
                Message = request.Message,
                PackageId = 0,
                Type = request.Type
            };
            RecordDispatch(dispatcher, package);
        }
    }
}
