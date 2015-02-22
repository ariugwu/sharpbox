using System.Runtime.CompilerServices;
using sharpbox.Log.Strategy;

namespace sharpbox.Log
{
    public class Client
    {
        #region Field(s)

        private IStrategy _strategy;

        #endregion

        #region Constructor(s)

        public Client(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public Client()
        {
        }

        #endregion

        #region Strategy Method(s)

        public T Exception<T>(T entity, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            return _strategy.Exception(entity, message, memberName, sourceFilePath, sourceLineNumber);
        }

        public T Warning<T>(T entity, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            return _strategy.Warning(entity, message, memberName, sourceFilePath, sourceLineNumber);
        }

        public T Info<T>(T entity, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            return _strategy.Info(entity, message, memberName, sourceFilePath, sourceLineNumber);
        }

        #endregion

    }
}
