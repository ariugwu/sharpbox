namespace sharpbox.Log.Strategy
{
    public interface IStrategy
    {
        void Exception(Dispatch.Client dispatcher, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);

        void Warning(Dispatch.Client dispatcher, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);

        void Info(Dispatch.Client dispatcher, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);

        void Trace(Dispatch.Client dispatcher, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);

        void LoadEntries(Dispatch.Client dispatcher);

        void SaveEntries(Dispatch.Client dispatcher);
    }
}
