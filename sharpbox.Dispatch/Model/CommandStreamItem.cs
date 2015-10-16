using System;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.Dispatch.Model
{
    [Serializable]
    public class CommandStreamItem
    {
        public CommandName Command { get; set; }
        public Response Response { get; set; }
    }
}
