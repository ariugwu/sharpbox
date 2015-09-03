using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sharpbox.Dispatch;
using sharpbox.Dispatch.Model;
using sharpbox.Web.Sharpbox.Data;

namespace sharpbox.Web.Sharpbox.Core.Strategies
{

    public class DefaultDispatchStrategy<T> : IDispatchStrategy<T>
    {

        public Dictionary<CommandNames, Dictionary<ResponseTypes, string>> CommandResponseMessageMap { get; set; }

        public IUnitOfWork<T> UnitOfWork { get; set; }

        public Feedback<T> Process(AppContext appContext, T instance, CommandNames commandName)
        {
            Response response = appContext.Dispatch.Process<T>(commandName, commandName.Name, new object[] { instance });

            return new Feedback<T>()
                       {
                           ResponseType = response.ResponseType,
                           Instance = (T)response.Entity,
                           Message = ""
                       };
        }

        public void RegisterCommands(AppContext appContext)
        {
            appContext.Dispatch.Register<T>(this.Insert, this.UnitOfWork.Insert, this.OnInsert);
            appContext.Dispatch.Register<T>(this.Update, this.UnitOfWork.Update, this.OnUpdate);
            appContext.Dispatch.Register<T>(this.Delete, this.UnitOfWork.Delete, this.OnDelete);
        }

        public void RegisterListeners(AppContext appContext)
        {
        }

        public void PopulateResponseMessageMap()
        {
            this.CommandResponseMessageMap = new Dictionary<CommandNames, Dictionary<ResponseTypes, string>>();

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

        public CommandNames Insert = new CommandNames("Insert");
        public CommandNames Update = new CommandNames("Update");
        public CommandNames Delete = new CommandNames("Delete");

        public EventNames OnInsert = new EventNames("OnInsert");
        public EventNames OnUpdate = new EventNames("OnUpdate");
        public EventNames OnDelete = new EventNames("OnDelete");

        #endregion
    }
}
