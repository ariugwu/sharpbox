using System.Collections.Generic;

namespace sharpbox.TestHarness
{
    public class Client
    {
        public Dictionary<Dispatch.Model.CommandStreamItem, string> TestStream { get; set; }

        private Dispatch.Client _dispatcher;

        public Client(Dispatch.Client dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Runner<T>(bool isPreview, T entity)
        {
            foreach(var c in _dispatcher.CommandHub)
            {
                //TODO: Find some way to mock the data in T
                _dispatcher.Process<T>(c.Key, "Test Runner: " + c.Value.Action.Method.Name, new object[]{ entity });
            }

            //TODO: Now that we have a populated command stream find a way to output that data nicely.
            //TODO: When data is output the (when) command (then) event should also show the (then) listeners.
            //TODO: Incorporate this ability to run the test against any previous frame. Since we know what the start and end should be could parse old data and reconstruct based on that alone.
        }
    }
}
