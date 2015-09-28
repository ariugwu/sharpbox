﻿using System.Collections.Generic;
using sharpbox.Dispatch.Model;
using sharpbox.WebLibrary.Core;
using sharpbox.WebLibrary.Data;

namespace sharpbox.Bootstrap.Package.Core.Strategies
{
    public abstract class BaseMediator<T> : IMediator<T> where T : new()
    {

        protected BaseMediator(AppContext appContext, IUnitOfWork<T> unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            this.BootstrapCommandsEventsRegistionsListeners(appContext);
        }

        protected BaseMediator(AppContext appContext)
          : this(appContext, new DefaultUnitOfWork<T>(appContext.File))
        {
        }

        public Dictionary<CommandName, Dictionary<ResponseTypes, string>> CommandMessageMap { get; set; }

        public IUnitOfWork<T> UnitOfWork { get; set; }

        public abstract void RegisterCommands(WebContext<T> webContext);

        public abstract void RegisterListeners(WebContext<T> webContext);

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

        private void BootstrapCommandsEventsRegistionsListeners(AppContext appContext)
        {
            //Register Command(s)
            appContext.Dispatch.Register<T>(this.Insert, this.UnitOfWork.Insert, this.OnInsert);
            appContext.Dispatch.Register<T>(this.Update, this.UnitOfWork.Update, this.OnUpdate);
            appContext.Dispatch.Register<T>(this.Delete, this.UnitOfWork.Delete, this.OnDelete);

            //Register Listener(s)

            //Populate Command DispatchResponse Map
            this.CommandMessageMap = new Dictionary<CommandName, Dictionary<ResponseTypes, string>>();

            this.CommandMessageMap.Add(this.Insert, new Dictionary<ResponseTypes, string>());
            this.CommandMessageMap[this.Insert].Add(ResponseTypes.Error, "Insert failed.");
            this.CommandMessageMap[this.Insert].Add(ResponseTypes.Info, "");
            this.CommandMessageMap[this.Insert].Add(ResponseTypes.Warning, "");
            this.CommandMessageMap[this.Insert].Add(ResponseTypes.Success, "Insert success.");

            this.CommandMessageMap.Add(this.Update, new Dictionary<ResponseTypes, string>());
            this.CommandMessageMap[this.Update].Add(ResponseTypes.Error, "Update failed.");
            this.CommandMessageMap[this.Update].Add(ResponseTypes.Info, "");
            this.CommandMessageMap[this.Update].Add(ResponseTypes.Warning, "");
            this.CommandMessageMap[this.Update].Add(ResponseTypes.Success, "Update success.");

            this.CommandMessageMap.Add(this.Delete, new Dictionary<ResponseTypes, string>());
            this.CommandMessageMap[this.Delete].Add(ResponseTypes.Error, "Update failed.");
            this.CommandMessageMap[this.Delete].Add(ResponseTypes.Info, "");
            this.CommandMessageMap[this.Delete].Add(ResponseTypes.Warning, "");
            this.CommandMessageMap[this.Delete].Add(ResponseTypes.Success, "Update success.");
        }
        #endregion
    }
}