using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using sharpbox.Dispatch.Model;
using sharpbox.Log.Model;

namespace sharpbox.Log
{
    public class Client
    {
        public Stack<Entry> LogStream { get { return _logStream ?? (_logStream = new Stack<Entry>()); } set { _logStream= value;}}
        private Stack<Entry> _logStream;

        private Response Log(Request request, EntryType entryType, string memberName,string sourceFilePath,int sourceLineNumber)
        {
            var entry = new Entry()
            {
                EntryType = entryType,
                EntryId = LogStream.Count + 1,
                Message = request.Message,
                MemberName = memberName,
                SourceFilePath = sourceFilePath,
                SourceLineNumber = sourceLineNumber,
                CreatedDate = DateTime.Now
            };

           LogStream.Push(entry);

           return new Response(request, "Entry logged", ResponseTypes.Success);
        }

        public Response Exception<T>(Request request, [CallerMemberName] string memberName = "",[CallerFilePath] string sourceFilePath = "",[CallerLineNumber] int sourceLineNumber = 0)
        {
            return Log(request, EntryType.Exception, memberName, sourceFilePath, sourceLineNumber);
        }

        public Response Warning(Request request, [CallerMemberName] string memberName = "",[CallerFilePath] string sourceFilePath = "",[CallerLineNumber] int sourceLineNumber = 0)
        {
            return Log(request, EntryType.Exception, memberName, sourceFilePath, sourceLineNumber);
        }

        public Response Info(Request request, [CallerMemberName] string memberName = "",[CallerFilePath] string sourceFilePath = "",[CallerLineNumber] int sourceLineNumber = 0)
        {
            return Log(request, EntryType.Exception, memberName, sourceFilePath, sourceLineNumber);
        }

    }
}
