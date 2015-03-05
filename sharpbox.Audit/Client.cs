using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Audit
{
    public class Client
    {
        private List<Response> _trail; 

        /// <summary>
        /// The Event Stream from all users
        /// </summary>
        public List<Response> Trail { get { return _trail ?? (_trail = new List<Response>()); } set { _trail = value; } } 

        public Client()
        {
            
        }

        public void Record(Response response)
        {
            Trail.Add(response);
        }

    }
}
