using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sharpbox.Web.Sharpbox.Core
{
    using sharpbox.Dispatch.Model;
    using sharpbox.Web.Sharpbox.Data;

    public interface IDispatchStrategy<T>
    {
        Dictionary<CommandNames, Dictionary<ResponseTypes, string>> CommandResponseMessageMap { get; set; }

        IUnitOfWork<T> UnitOfWork { get; set; }

        Feedback<T> Process(AppContext appContext, T instance, CommandNames commandName);

        void RegisterCommands(AppContext appContext);

        void RegisterListeners(AppContext appContext);

        void PopulateResponseMessageMap();
    }
}
