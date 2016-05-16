using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Dispatch.Model;

namespace sharpbox.TestHarness
{
    public class Client
    {
        public List<IResponse> TestStream { get; set; }

        public List<IResponse> TestResponses {
            get
            {
                return _dispatcher.CommandStream.Where(x => TestStream.Contains(x.Response)).Select(x => x.Response).ToList();
            }
        } 

        private Dispatch.DispatchContext _dispatcher;

        public Client(Dispatch.DispatchContext dispatcher)
        {
            _dispatcher = dispatcher;
            TestStream = new List<IResponse>();
        }

        public void Runner<T>(bool isPreview, T entity)
        {
            foreach (var response in _dispatcher.CommandHub.Select(c => _dispatcher.Process<T>(c.Key, "Test Runner: " + c.Value.Action.Method.Name, new object[]{ entity })))
            {
                this.TestStream.Add(response);
            }
            //TODO: Now that we have a populated command stream find a way to output that data nicely.
            //TODO: When data is output the (when) command (then) event should also show the (then) listeners.
            //TODO: Incorporate this ability to run the test against any previous frame. Since we know what the start and end should be could parse old data and reconstruct based on that alone.
        }

      public byte[] GenerateExcelBddFile(Dispatch.DispatchContext dispatcher)
      {
        throw new NotImplementedException();
      }
    }
}
