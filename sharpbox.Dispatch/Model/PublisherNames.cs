using System.Collections.Generic;

namespace sharpbox.Dispatch.Model
{
    public class PublisherNames
    {

        #region Available Publishers List

        #region Audit
        public static readonly PublisherNames OnAuditRecord = new PublisherNames("OnAuditRecord");
        public static readonly PublisherNames OnAuditPersist = new PublisherNames("OnAuditPersist");
        public static readonly PublisherNames OnAuditLoad = new PublisherNames("OnAuditLoad");
        #endregion

        #region Email
        public static readonly PublisherNames OnEmailSend = new PublisherNames("OnEmailSend");
        #endregion

        #region Io
        public static readonly PublisherNames OnFileCreate = new PublisherNames("OnFileCreate");
        public static readonly PublisherNames OnFileDelete = new PublisherNames("OnFileDelete");
        public static readonly PublisherNames OnFileAccess = new PublisherNames("OnFileAccess");
        #endregion

        #region Log

        public static readonly PublisherNames OnLogTrace = new PublisherNames("OnLogTrace");
        public static readonly PublisherNames OnLogInfo = new PublisherNames("OnLogInfo");
        public static readonly PublisherNames OnLogWarning = new PublisherNames("OnLogWarning");
        public static readonly PublisherNames OnLogException = new PublisherNames("OnLogException");

        #endregion

        #region Data

        public static readonly PublisherNames OnDataCreate = new PublisherNames("OnDataCreate");
        public static readonly PublisherNames OnDataUpdate = new PublisherNames("OnDataUpdate");
        public static readonly PublisherNames OnDataDelete = new PublisherNames("OnDataDelete");
        public static readonly PublisherNames OnDataGetById = new PublisherNames("OnDataGetById");
        public static readonly PublisherNames OnDataGetAll = new PublisherNames("OnDataGetAll");

        #endregion

        #region Membership

        public static readonly PublisherNames OnUserChange = new PublisherNames("OnUserChange");

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
                OnLogException,
                OnFileCreate,
                OnFileDelete,
                OnFileAccess,
                OnEmailSend,
                OnAuditRecord,
                OnUserChange
            };
        } 

        #endregion
    }
}
