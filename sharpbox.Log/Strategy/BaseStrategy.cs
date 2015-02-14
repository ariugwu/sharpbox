using System;

namespace sharpbox.Log.Strategy
{
    public class BaseStrategy : IStrategy
    {
        public void Exception(string message,string memberName = "",string sourceFilePath = "",int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        public void Info(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
        }
    }
}
