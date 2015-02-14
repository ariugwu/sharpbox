using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using sharpbox.Log.Strategy;

namespace sharpbox.Log
{
    public class Client
    {
        #region Field(s)

        private IStrategy _strategy;

        #endregion

        #region Properties

        private IStrategy Strategy
        {
            get
            {
                if (_strategy != null) return _strategy;

                _strategy = new BaseStrategy();
                _strategy.Trace("Logging module created without supplying a logging strategy. Defaulting to the base XML.");

                return _strategy;
            }
            set { _strategy = value; }
        }

        #endregion

        #region Constructor(s)

        public Client(IStrategy strategy)
        {
            Strategy = strategy;
        }

        public Client()
        {
        }

        #endregion

        #region Strategy Method(s)

        public void Exception(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            _strategy.Exception(message, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Warning(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            _strategy.Warning(message, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Info(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            _strategy.Info(message, memberName, sourceFilePath, sourceLineNumber);
        }

        [Conditional("DEBUG")]
        public void Trace(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            _strategy.Trace(message, memberName, sourceFilePath, sourceLineNumber);
            Debug.WriteLine(String.Format("TRACE: {0}", message)); // Write to the output window in visual studio
        }
        #endregion

    }
}
