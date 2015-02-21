using System;
using System.Collections.Generic;
using sharpbox.Log.Model;

namespace sharpbox.Log.Strategy.File
{
    public class FileStrategy: IStrategy
    {
        private Io.Client _file;
        private Dictionary<string, object> _props;

        public FileStrategy(Dispatch.Client dispatcher, Io.Strategy.IStrategy persistenceStrategy, Dictionary<string, object> props)
        {
            _file = new Io.Client(persistenceStrategy);
            _props = props;

            LoadEntries(dispatcher);
        }

        public List<Entry> Entries { get; set; }

        public void Exception(Dispatch.Client dispatcher, string message, string memberName = "",
            string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Entries.Add(new Entry()
            {
                EntryType = EntryType.Exception,
                EntryId = Entries.Count + 1,
                Message = message,
                MemberName = memberName,
                SourceFilePath = sourceFilePath,
                SourceLineNumber = sourceLineNumber,
                CreatedDate = DateTime.Now
            });
            SaveEntries(dispatcher);
        }

        public void Warning(Dispatch.Client dispatcher, string message, string memberName = "",
            string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Entries.Add(new Entry()
            {
                EntryType = EntryType.Warning,
                EntryId = Entries.Count + 1,
                Message = message,
                MemberName = memberName,
                SourceFilePath = sourceFilePath,
                SourceLineNumber = sourceLineNumber,
                CreatedDate = DateTime.Now
            });
            SaveEntries(dispatcher);
        }

        public void Info(Dispatch.Client dispatcher, string message, string memberName = "", string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            Entries.Add(new Entry()
            {
                EntryType = EntryType.Info,
                EntryId = Entries.Count + 1,
                Message = message,
                MemberName = memberName,
                SourceFilePath = sourceFilePath,
                SourceLineNumber = sourceLineNumber,
                CreatedDate = DateTime.Now
            });
            SaveEntries(dispatcher);
        }

        public void Trace(Dispatch.Client dispatcher, string message, string memberName = "", string sourceFilePath = "",
            int sourceLineNumber = 0)
        {
            Entries.Add(new Entry()
            {
                EntryType = EntryType.Trace,
                EntryId = Entries.Count + 1,
                Message = message,
                MemberName = memberName,
                SourceFilePath = sourceFilePath,
                SourceLineNumber = sourceLineNumber,
                CreatedDate = DateTime.Now
            });
            SaveEntries(dispatcher);
        }

        public void LoadEntries(Dispatch.Client dispatcher)
        {
            var filePath = _props["filePath"].ToString();
            if (!_file.Exists(filePath)) _file.Write(dispatcher, filePath, new List<Entry>());
            Entries = _file.Read<List<Entry>>(dispatcher, _props["filePath"].ToString());
        }

        public void SaveEntries(Dispatch.Client dispatcher)
        {
            _file.Write(dispatcher, _props["filePath"].ToString(), Entries);
            LoadEntries(dispatcher);
        }
    }
}
