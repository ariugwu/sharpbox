using System;
using System.Collections.Generic;
using sharpbox.Log.Model;

namespace sharpbox.Log.Strategy.File
{
    public class FileStrategy: IStrategy
    {
        private Io.Client _file;
        private Dictionary<string, object> _props;

        public FileStrategy(Io.Strategy.IStrategy persistenceStrategy, Dictionary<string, object> props)
        {
            _file = new Io.Client(persistenceStrategy);
            _props = props;

            LoadEntries();
        }

        public List<Entry> Entries { get; set; }

        public T Exception<T>(T exception, string message, string memberName = "",
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
            SaveEntries();
            return exception;
        }

        public T Warning<T>(T exception, string message, string memberName = "",
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
            SaveEntries();
            return exception;
        }

        public T Info<T>(T exception, string message, string memberName = "", string sourceFilePath = "",
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
            SaveEntries();
            return exception;
        }


        public void LoadEntries()
        {
            var filePath = _props["filePath"].ToString();
            if (!_file.Exists(filePath)) _file.Write(filePath, new List<Entry>());
            Entries = _file.Read<List<Entry>>(_props["filePath"].ToString());
        }

        public void SaveEntries()
        {
            _file.Write(_props["filePath"].ToString(), Entries);
            LoadEntries();
        }
    }
}
