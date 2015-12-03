using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.Common.Dispatch
{
    using sharpbox.Common.Dispatch.Model;

    public interface ICommandStreamItem
    {
        CommandName Command { get; set; }

        IResponse Response { get; set; }
    }
}
