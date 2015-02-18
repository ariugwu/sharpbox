using System;
using System.Collections.Generic;
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

        #region Constructor(s)

        public Client(Dispatch.Client dispatcher, IStrategy strategy = null, Dictionary<string, object> props = null)
        {
            _strategy = strategy ?? new BaseStrategy(dispatcher, props ?? new Dictionary<string, object> { { "xmlPath", "LogXmlRepository.xml" } });
        }

        public Client()
        {
        }

        #endregion

        #region Strategy Method(s)

        public void Exception(Dispatch.Client dispatcher, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            _strategy.Exception(dispatcher, message, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Warning(Dispatch.Client dispatcher, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            _strategy.Warning(dispatcher, message, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Info(Dispatch.Client dispatcher, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            _strategy.Info(dispatcher, message, memberName, sourceFilePath, sourceLineNumber);
        }

        [Conditional("DEBUG")]
        public void Trace(Dispatch.Client dispatcher, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            _strategy.Trace(dispatcher, message, memberName, sourceFilePath, sourceLineNumber);
            Debug.WriteLine(String.Format("TRACE: {0}", message)); // Write to the output window in visual studio
        }
        #endregion

    }
}
