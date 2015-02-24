using System.Collections.Generic;
using sharpbox.Data.Strategy;

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
            return e;
        }

        public T Get(Dispatch.Client dispatcher, int id)
        {
            var e = _strategy.Get(dispatcher, id);
            return e;
        }

        public T Update(Dispatch.Client dispatcher, T entity)
        {
            var e = _strategy.Update(dispatcher, entity);
            return e;
        }

        public IEnumerable<T> UpdateAll(Dispatch.Client dispatcher, IEnumerable<T> entities)
        {
            var e = _strategy.UpdateAll(dispatcher, entities);
            return e;
        }

        public void Delete(Dispatch.Client dispatcher, T entity)
        {
            _strategy.Delete(dispatcher, entity);

        }

    }
}
