using System.Collections.Generic;

namespace sharpbox.Dispatch.Model
{
    public class PublisherNames
    {

        #region Available Publishers List

        #region Audit

        #endregion

        #region Email

        #endregion

        #region Io

        #endregion

        #region Log

        public static readonly PublisherNames OnLogTrace = new PublisherNames("OnLogTrace");
        public static readonly PublisherNames OnLogInfo = new PublisherNames("OnLogInfo");
        public static readonly PublisherNames OnLogWarning = new PublisherNames("OnLogWarning");
        public static readonly PublisherNames OnLogException = new PublisherNames("OnLogException");

        #endregion

        #region Membership

        #endregion

        #endregion

        #region Field(s)

        protected string _value;

        #endregion


        #region Constructor(s)
        public PublisherNames(string value)
        {
            _value = value;
        }

        public PublisherNames() { }

        #endregion


        #region Override(s)
        public override string ToString()
        {
            return _value;
        }

        #endregion

        #region Method(s)

        public static List<PublisherNames> DefaultPubList()
        {
            return new List<PublisherNames>()
            {
                OnLogTrace,
                OnLogInfo,
                OnLogWarning,
                OnLogException
            };
        } 

        #endregion
    }
}
