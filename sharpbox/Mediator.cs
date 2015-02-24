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

    }
}
