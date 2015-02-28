using sharpbox.Dispatch;

namespace sharpbox
{
    public abstract class Mediator
    {
        /// <summary>
        /// This constructor will do some wiring for you.
        /// </summary>
        protected Mediator()
        {
            // Pub/Sub System(s)
            Dispatch = new Client();
        }

        public Client Dispatch { get; set; }


        #region Email Call(s)

        #endregion

        #region IO Call(s)

        #endregion

        #region Localization Call(s)

        #endregion

        #region Security Call(s)

        #endregion
    }
}
