using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Data;
using sharpbox.Data.Strategy;
using sharpbox.Log.Model;

namespace sharpbox.Log.Strategy
{
    public class BaseStrategy : IStrategy
    {

        public BaseStrategy(Dispatch.Client dispatcher,Dictionary<string, object> props)
        {
            Repository = new Repository<Entry>(dispatcher, new XmlStrategy<Entry>(dispatcher,props), props);
            LoadEntries(dispatcher);
        }

        public Repository<Entry> Repository { get; set; }

        public List<Entry> Entries { get; set; } 

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
            Entries = Repository.All(dispatcher).ToList();
        }

        public void SaveEntries(Dispatch.Client dispatcher)
        {
            Repository.UpdateAll(dispatcher, Entries);
            LoadEntries(dispatcher);
        }
    }
}
