using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Dispatch.Model;

namespace sharpbox.TestHarness
{
    public class Client
    {
        public List<Guid> TestStream { get; set; }

        public List<Response> TestResponses {
            get
            {
                return _dispatcher.CommandStream.Where(x => TestStream.Contains(x.Response.RequestSharpId)).Select(x => x.Response).ToList();
            }
        } 

        private Dispatch.Client _dispatcher;

        public Client(Dispatch.Client dispatcher)
        {
            _dispatcher = dispatcher;
            TestStream = new List<Guid>();
        }

        public void Runner<T>(bool isPreview, T entity)
        {
            foreach (var response in _dispatcher.CommandHub.Select(c => _dispatcher.Process<T>(c.Key, "Test Runner: " + c.Value.Action.Method.Name, new object[]{ entity })))
            {
                this.TestStream.Add(response.RequestSharpId);
            }
            //TODO: Now that we have a populated command stream find a way to output that data nicely.
            //TODO: When data is output the (when) command (then) event should also show the (then) listeners.
            //TODO: Incorporate this ability to run the test against any previous frame. Since we know what the start and end should be could parse old data and reconstruct based on that alone.
        }

      public byte[] GenerateExcelBddFile(Dispatch.Client dispatcher)
      {
        throw new NotImplementedException();
      }
    }
}
