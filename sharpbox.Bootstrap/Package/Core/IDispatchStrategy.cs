using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Data;

namespace sharpbox.WebLibrary.Core
{

    public interface IDispatchStrategy<T>
    {
        Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandResponseMessageMap { get; set; }

        IUnitOfWork<T> UnitOfWork { get; set; }

        void RegisterCommands(WebContext<T> webContext);

        void RegisterListeners(WebContext<T> webContext);

        void PopulateResponseMessageMap();
    }
}
