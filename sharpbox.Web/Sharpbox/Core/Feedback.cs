using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sharpbox.Web.Sharpbox.Core
{
    public class Feedback<T>
    {
        public T Instance { get; set; }
        public string Message { get; set; }
        public FeedbackType FeedbackType { get; set; }
    }
}
