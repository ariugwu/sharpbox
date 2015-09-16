using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Data;

namespace sharpbox.WebLibrary.Core.Strategies
{

  public class DefaultDispatchStrategy<T> : IDispatchStrategy<T>
  {

    public DefaultDispatchStrategy(IUnitOfWork<T> unitOfWork)
    {
      UnitOfWork = unitOfWork;
    }

    public DefaultDispatchStrategy()
      : this(new DefaultUnitOfWork<T>())
    {
    }

    public Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandResponseMessageMap { get; set; }

    public Data.IUnitOfWork<T> UnitOfWork { get; set; }

    public void RegisterCommands(WebContext<T> webContext)
    {
      webContext.AppContext.Dispatch.Register<T>(this.Insert, this.UnitOfWork.Insert, this.OnInsert);
      webContext.AppContext.Dispatch.Register<T>(this.Update, this.UnitOfWork.Update, this.OnUpdate);
      webContext.AppContext.Dispatch.Register<T>(this.Delete, this.UnitOfWork.Delete, this.OnDelete);
    }

    public void RegisterListeners(WebContext<T> webContext)
    {
    }

    public void PopulateResponseMessageMap()
    {
      this.CommandResponseMessageMap = new Dictionary<CommandName, Dictionary<ResponseTypes, string>>();

      this.CommandResponseMessageMap.Add(this.Insert, new Dictionary<ResponseTypes, string>());
      this.CommandResponseMessageMap[this.Insert].Add(ResponseTypes.Error, "");
      this.CommandResponseMessageMap[this.Insert].Add(ResponseTypes.Info, "");
      this.CommandResponseMessageMap[this.Insert].Add(ResponseTypes.Warning, "");
      this.CommandResponseMessageMap[this.Insert].Add(ResponseTypes.Success, "");

      this.CommandResponseMessageMap.Add(this.Update, new Dictionary<ResponseTypes, string>());
      this.CommandResponseMessageMap[this.Update].Add(ResponseTypes.Error, "");
      this.CommandResponseMessageMap[this.Update].Add(ResponseTypes.Info, "");
      this.CommandResponseMessageMap[this.Update].Add(ResponseTypes.Warning, "");
      this.CommandResponseMessageMap[this.Update].Add(ResponseTypes.Success, "");

      this.CommandResponseMessageMap.Add(this.Delete, new Dictionary<ResponseTypes, string>());
      this.CommandResponseMessageMap[this.Delete].Add(ResponseTypes.Error, "");
      this.CommandResponseMessageMap[this.Delete].Add(ResponseTypes.Info, "");
      this.CommandResponseMessageMap[this.Delete].Add(ResponseTypes.Warning, "");
      this.CommandResponseMessageMap[this.Delete].Add(ResponseTypes.Success, "");

    }
    #region Commands and Events

    public CommandName Insert = new CommandName("Insert");
    public CommandName Update = new CommandName("Update");
    public CommandName Delete = new CommandName("Delete");

    public EventName OnInsert = new EventName("OnInsert");
    public EventName OnUpdate = new EventName("OnUpdate");
    public EventName OnDelete = new EventName("OnDelete");

    #endregion
  }
}
