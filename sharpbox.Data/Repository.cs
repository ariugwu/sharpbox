using System;
using System.Collections.Generic;
using sharpbox.Data.Strategy;
using sharpbox.Dispatch.Model;

namespace sharpbox.Data
{
    public class Repository<T>
    {

        private IStrategy<T> _strategy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strategy">A persistence strategy is required.</param>
        public Repository(IStrategy<T> strategy)
        {
            _strategy = strategy;
        }

        public Repository()
        {

        }

        public IEnumerable<T> All(Dispatch.Client dispatcher)
        {
            return _strategy.All(dispatcher);
        }

        public T Create(Dispatch.Client dispatcher, T entity)
        {
            var e = _strategy.Create(dispatcher, entity);
            dispatcher.Broadcast(new Package() { EventName = EventNames.OnDataCreate, Message = "Entity created.", Entity = e, Type = this.GetType(), PackageId = Guid.NewGuid(), UserId = dispatcher.CurrentUserId });
            return e;
        }

        public T Get(Dispatch.Client dispatcher, int id)
        {
            var e = _strategy.Get(dispatcher, id);
            dispatcher.Broadcast(new Package() { EventName = EventNames.OnDataCreate, Message = "Entity retrieved", Entity = e, Type = this.GetType(), PackageId = Guid.NewGuid(), UserId = dispatcher.CurrentUserId });
            return e;
        }

        public T Update(Dispatch.Client dispatcher, T entity)
        {
            var e = _strategy.Update(dispatcher, entity);
            dispatcher.Broadcast(new Package() { EventName = EventNames.OnDataCreate, Message = "Entity updated", Entity = e, Type = this.GetType(), PackageId = Guid.NewGuid(), UserId = dispatcher.CurrentUserId });
            return e;
        }

        public IEnumerable<T> UpdateAll(Dispatch.Client dispatcher, IEnumerable<T> entities)
        {
            var e = _strategy.UpdateAll(dispatcher, entities);
            dispatcher.Broadcast(new Package() { EventName = EventNames.OnDataCreate, Message = "All entities updated", Entity = e, Type = this.GetType(), PackageId = Guid.NewGuid(), UserId = dispatcher.CurrentUserId });
            return e;
        }

        public void Delete(Dispatch.Client dispatcher, T entity)
        {
            _strategy.Delete(dispatcher, entity);
            dispatcher.Broadcast(new Package() { EventName = EventNames.OnDataCreate, Message = "Entity deleted", Entity = entity, Type = this.GetType(), PackageId = Guid.NewGuid(), UserId = dispatcher.CurrentUserId });
        }

    }
}
