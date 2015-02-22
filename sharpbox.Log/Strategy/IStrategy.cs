namespace sharpbox.Log.Strategy
{
    public interface IStrategy
    {
        T Exception<T>(T exception, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);

        T Warning<T>(T exception, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);

        T Info<T>(T exception, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);

        void LoadEntries();

        void SaveEntries();
    }
}
