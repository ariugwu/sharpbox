using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Data;
using sharpbox.Log.Model;

namespace sharpbox.Log.Strategy
{
    public class BaseStrategy : IStrategy
    {

        public BaseStrategy(Dispatch.Client dispatcher,
            Dictionary<string, object> auxInfo)
        {
            AuxInfo = auxInfo;
            _xmlPath = (string)AuxInfo["xmlPath"];
            _repository = new Repository<Entry>(dispatcher, auxInfo: auxInfo);
        }

        private string _xmlPath;
        private Repository<Entry> _repository;
        private List<Entry> _entries;
 
        public Repository<Entry> Repository
        {
            get { return _repository; }
            set
            {
                _repository = value;
            }
        }

        public List<Entry> Entries { get { return _entries ?? (_entries = new List<Entry>()); } } 

        public Dictionary<string, object> AuxInfo { get; set; }

        public void Exception(Dispatch.Client dispatcher, string message,string memberName = "",string sourceFilePath = "",int sourceLineNumber = 0)
        {
            Entries.Add(new Entry(){ EntryType = EntryType.Exception, EntryId = Entries.Count + 1, Message = message, MemberName = memberName, SourceFilePath = sourceFilePath, SourceLineNumber = sourceLineNumber, CreatedDate = DateTime.Now});
            SaveEntries(dispatcher);
        }

        public void Warning(Dispatch.Client dispatcher, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Entries.Add(new Entry() { EntryType = EntryType.Warning, EntryId = Entries.Count + 1, Message = message, MemberName = memberName, SourceFilePath = sourceFilePath, SourceLineNumber = sourceLineNumber, CreatedDate = DateTime.Now });
            SaveEntries(dispatcher);
        }

        public void Info(Dispatch.Client dispatcher, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Entries.Add(new Entry() { EntryType = EntryType.Info, EntryId = Entries.Count + 1, Message = message, MemberName = memberName, SourceFilePath = sourceFilePath, SourceLineNumber = sourceLineNumber, CreatedDate = DateTime.Now });
            SaveEntries(dispatcher);
        }

        public void Trace(Dispatch.Client dispatcher, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Entries.Add(new Entry() { EntryType = EntryType.Trace, EntryId = Entries.Count + 1, Message = message, MemberName = memberName, SourceFilePath = sourceFilePath, SourceLineNumber = sourceLineNumber, CreatedDate = DateTime.Now });
            SaveEntries(dispatcher);
        }

        public void LoadEntries(Dispatch.Client dispatcher)
        {
            _entries = Repository.All(dispatcher).ToList();
        }

        public void SaveEntries(Dispatch.Client dispatcher)
        {
            Repository.UpdateAll(dispatcher, Entries);
            LoadEntries(dispatcher);
        }
    }
}
