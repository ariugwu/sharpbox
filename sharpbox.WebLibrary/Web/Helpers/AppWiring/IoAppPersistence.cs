using System.Web.Http.OData.Query;

namespace sharpbox.WebLibrary.Web.Helpers.AppWiring
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using App;
    using Core.Wiring;

    public class IoAppPersistence : IAppPersistence
    {
        public AppContext AppContext { get; set; }

        public T Add<T>(T instance) where T : new()
        {
            return this.Update(instance);
        }

        public List<T> UpdateAll<T>(List<T> items) where T : new()
        {
            var path = Path.Combine(this.AppContext.DataPath, $"{typeof(T).Name}.dat");

            if (this.AppContext.File.Exists(path))
            {
                this.AppContext.File.Replace(path, items);
            }
            else
            {
                items = new List<T>();

                this.AppContext.File.Write(path, items);
            }

            return items;
        }

        public T Update<T>(T instance) where T : new()
        {
            PropertyInfo idInfo = Common.Type.TypeInfoHelper.GetIdPropertyByConvention(typeof(T));

            if (idInfo != null)
            {
                List<T> things = this.Get<T>(null).ToList();

                //@SEE: http://stackoverflow.com/a/28658501
                var item = things.FirstOrDefault(x => idInfo.GetValue(x).ToString() == idInfo.GetValue(instance).ToString());

                if (item != null)
                {
                    things[things.IndexOf(item)] = instance;
                }
                else
                {
                    things.Add(instance);
                }

                var path = Path.Combine(this.AppContext.DataPath, string.Format("{0}.dat", typeof(T).Name));

                if (this.AppContext.File.Exists(path))
                {
                    this.AppContext.File.Replace(path, things);
                }
                else
                {
                    this.AppContext.File.Write(path, things);
                }

                return instance;
            }
            throw new ArgumentException("This object has no id that follows the connvention 'ObjectNameId'");
        }

        public T Remove<T>(T instance) where T : new()
        {
            instance = new T();

            return this.Update(instance);
        }

        public T GetById<T>(string id) where T : new()
        {
            PropertyInfo idInfo = Common.Type.TypeInfoHelper.GetIdPropertyByConvention(typeof(T));
            
            if (idInfo != null)
            {
                List<T> things = this.Get<T>(null).ToList();

                //@SEE: http://stackoverflow.com/a/28658501
                return things.FirstOrDefault(x => idInfo.GetValue(x).ToString() == id);
            }

            throw new ArgumentException("This object has no id that follows the connvention 'ObjectNameId'");
        }

        public IQueryable<T> Get<T>(object arg) where T : new()
        {
            List<T> things;

            var path = Path.Combine(this.AppContext.DataPath, string.Format("{0}.dat", typeof(T).Name));

            if (this.AppContext.File.Exists(path))
            {
                things = this.AppContext.File.Read<List<T>>(path);
            }
            else
            {
                things = new List<T>();

                this.AppContext.File.Write(path, things);
            }

            IQueryable<T> results;

            if (arg != null)
            {
                var oDataOptions = (ODataQueryOptions<T>) arg;
                var queryResults = oDataOptions.ApplyTo(things.AsQueryable());

                results = (IQueryable<T>)queryResults;

            }
            else
            {
                results = things.AsQueryable();
            }

            return results;
        }
    }
}