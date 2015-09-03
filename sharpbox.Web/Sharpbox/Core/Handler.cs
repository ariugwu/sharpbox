using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sharpbox.Web.Sharpbox.Core
{
    public abstract class Handler
    {
        protected Handler _successor;

        /// <summary>
        /// The set successor.
        /// </summary>
        /// <param name="successor">
        /// The successor.
        /// </param>
        public void SetSuccessor(Handler successor)
        {
            this._successor = successor;
        }

        public abstract AppContext HandleRequest(AppContext appContext);
    }
}
