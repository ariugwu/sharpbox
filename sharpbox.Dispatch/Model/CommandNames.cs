using System;
using System.Collections.Generic;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandNames
    {
        #region Constructor(s)

        public CommandNames(string value)
        {
            _value = value;
        }

        public CommandNames() { }

        #endregion

        #region Available Actions List

        public static readonly CommandNames RegisterAction = new CommandNames("RegisterAction");

        #region Feedback

        public static readonly CommandNames SetFeedback = new CommandNames("SetFeedback");

        #endregion

        #region Membership

        public static readonly CommandNames ChangeUser = new CommandNames("ChangeUser");

        #endregion

        #endregion

        #region Field(s)

        protected string _value;

        #endregion

        #region Override(s)
        public override string ToString()
        {
            return _value;
        }

        #endregion

        #region Method(s)

        public static List<CommandNames> DefaultActionList()
        {
            return new List<CommandNames>()
            {
                SetFeedback
            };
        }

        #endregion
    }
}
