using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            // The benefit of the dispatcher is being able to see all subscribed events in one place at one time.
            // This is centeralization is put to use with the Audit component which, when set to AuditLevel = All, will make a entry for *every* registered system event.
            var app = new AppContext();
        }
    }
}
