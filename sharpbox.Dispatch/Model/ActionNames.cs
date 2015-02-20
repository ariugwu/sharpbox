using System.Collections.Generic;

namespace sharpbox.Dispatch.Model
{
    public class ActionNames
    {
        #region Constructor(s)

        public ActionNames(string value)
        {
            _value = value;
        }

        public ActionNames() { }

        #endregion

        #region Available Publishers List

        public static readonly ActionNames RegisterAction = new ActionNames("RegisterAction");

        #region Feedback

        public static readonly ActionNames SetFeedback = new ActionNames("SetFeedback");

        #endregion

        #region Membership

        public static readonly ActionNames ChangeUser = new ActionNames("ChangeUser");

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

        public static List<ActionNames> DefaultActionList()
        {
            return new List<ActionNames>()
            {
                SetFeedback
            };
        }

        #endregion
    }
}
