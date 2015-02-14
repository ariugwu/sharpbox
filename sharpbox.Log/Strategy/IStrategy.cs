namespace sharpbox.Log.Strategy
{
    public interface IStrategy
    {
        void Exception(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);

        void Warning(string message,string memberName = "",string sourceFilePath = "",int sourceLineNumber = 0);

        void Info(string message,string memberName = "",string sourceFilePath = "",int sourceLineNumber = 0);

        void Trace(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);
    }
}
