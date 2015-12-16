namespace sharpbox.App.AppWiring
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http.OData.Query;

    using Common.App;
    using Common.Io;

    public class IoCrud : ICrud
    {
        public Io.Client File { get; set; }

        public T Add<T>(T instance) where T : new()
        {
            return this.UpdateById(instance);
        }

        public List<T> Update<T>(List<T> items) where T : new()
        {
            var path = Path.Combine(this.File.DataPath, $"{typeof(T).Name}.dat");

            if (this.File.Exists(path))
            {
                this.File.Replace(path, items);
            }
            else
            {
                items = new List<T>();

                this.File.Write(path, items);
            }

            return items;
        }

        public T UpdateById<T>(T instance) where T : new()
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

                var path = Path.Combine(this.File.DataPath, string.Format("{0}.dat", typeof(T).Name));

                if (this.File.Exists(path))
                {
                    this.File.Replace(path, things);
                }
                else
                {
                    this.File.Write(path, things);
                }

                return instance;
            }
            throw new ArgumentException("This object has no id that follows the connvention 'ObjectNameId'");
        }

        public T Remove<T>(T instance) where T : new()
        {
            instance = new T();

            return this.UpdateById(instance);
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

            var path = Path.Combine(this.File.DataPath, string.Format("{0}.dat", typeof(T).Name));

            if (this.File.Exists(path))
            {
                things = this.File.Read<List<T>>(path);
            }
            else
            {
                things = new List<T>();

                this.File.Write(path, things);
            }
            
            return things.AsQueryable();
        }
    }
}