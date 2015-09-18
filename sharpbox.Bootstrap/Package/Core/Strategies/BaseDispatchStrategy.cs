using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Data;

namespace sharpbox.Bootstrap.Package.Core.Strategies
{
  public abstract class BaseDispatchStrategy<T> : IDispatchStrategy<T>
  {
    
    protected BaseDispatchStrategy(WebContext<T> webContext, IUnitOfWork<T> unitOfWork)
    {
      UnitOfWork = unitOfWork;

      BootstrapCommandsEventsRegistionsListeners(webContext);
    }

    protected BaseDispatchStrategy(WebContext<T> webContext)
      : this(webContext, new DefaultUnitOfWork<T>())
    {
    }

    public Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandResponseMessageMap { get; set; }

    public IUnitOfWork<T> UnitOfWork { get; set; }

    public virtual void RegisterCommands(WebContext<T> webContext)
    {
    }

    public virtual void RegisterListeners(WebContext<T> webContext)
    {
    }

    public virtual void PopulateResponseMessageMap()
    {

    }

    #region Commands and Events

    public CommandName Insert = new CommandName("Insert");
    public CommandName Update = new CommandName("Update");
    public CommandName Delete = new CommandName("Delete");

    public EventName OnInsert = new EventName("OnInsert");
    public EventName OnUpdate = new EventName("OnUpdate");
    public EventName OnDelete = new EventName("OnDelete");

    #endregion

    #region Bootstrap methods

    private void BootstrapCommandsEventsRegistionsListeners(WebContext<T> webContext)
    {
      //Register Command(s)
      webContext.AppContext.Dispatch.Register<T>(this.Insert, this.UnitOfWork.Insert, this.OnInsert);
      webContext.AppContext.Dispatch.Register<T>(this.Update, this.UnitOfWork.Update, this.OnUpdate);
      webContext.AppContext.Dispatch.Register<T>(this.Delete, this.UnitOfWork.Delete, this.OnDelete);

      //Register Listener(s)

      //Populate Command Response Map
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
    #endregion
  }
}