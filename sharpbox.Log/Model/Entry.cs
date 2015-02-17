using System;

namespace sharpbox.Log.Model
{
    public class Entry
    {
        public int EntryId { get; set; }
        public EntryType EntryType { get; set; }
        public string Message { get; set; }
        public string MemberName { get; set; } 
        public string SourceFilePath { get; set; } 
        public int SourceLineNumber { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
