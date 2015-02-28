using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit.Strategy
{
    public class InMemoryStrategy : IStrategy
    {
        private List<Response> _trail;

        public List<Response> Trail
        {
            get { return _trail ?? (_trail = new List<Response>());}
            set { _trail = value; }
        }

        public void Record(Response response)
        {
            Trail.Add(response);
        }
    }
}
