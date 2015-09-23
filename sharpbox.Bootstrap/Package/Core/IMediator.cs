using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Data;

namespace sharpbox.WebLibrary.Core
{

    public interface IMediator<T> where T : new()
    {
        Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandMessageMap { get; set; }

        IUnitOfWork<T> UnitOfWork { get; set; }

        void RegisterCommands(WebContext<T> webContext);

        void RegisterListeners(WebContext<T> webContext);

        void PopulateResponseMessageMap();
    }
}
