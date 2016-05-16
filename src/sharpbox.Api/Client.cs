using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using sharpbox.App;
using sharpbox.Dispatch.Model;

namespace sharpbox.Api
{
    public class Client<T> where T : class
    {
        #region Constructor(s)

        public Client(AppContext defaultContext)
        {
            _context = defaultContext;

            this._odataModelbuilder.EntitySet<T>($"{typeof(T).Name}s");
            this._odataModelbuilder.AddEntity(typeof(T));
        } 

        #endregion

        #region Field(s)

        private readonly ODataConventionModelBuilder _odataModelbuilder = new ODataConventionModelBuilder();
        
        private readonly AppContext _context = new AppContext();

        private readonly string _dummyAbsoluteUri = string.Empty;

        #endregion

        #region API

        public IQueryable<T> Get(string query)
        {
            return this.Query(AppContext.Get, query).AsQueryable();
        }

        public IResponse Put(T entity)
        {
            return this.Execute(AppContext.Add, new object[] { entity});
        }

        public IResponse Post(T entity)
        {
            return this.Execute(AppContext.Update, new object[] { entity });
        }

        public IResponse Delete(T entity)
        {
            return this.Execute(AppContext.Remove, new object[] { entity });
        }

        #endregion

        #region CQRS

        public IList<T> Query(QueryName queryName, string query = null)
        {
            // Get the full QueryName
            queryName = _context.Dispatch.QueryHub.First(x => x.Key.Name == queryName.Name).Key;

            ODataQueryOptions<T> options = null;
            List<object> args = null;

            if (!string.IsNullOrEmpty(query))
            {
                options = new ODataQueryOptions<T>(new ODataQueryContext(this._odataModelbuilder.GetEdmModel(), typeof(T)), new HttpRequestMessage(HttpMethod.Get, _dummyAbsoluteUri));
                args.Add(options);
            }
            
            if(args == null)
            {
                return _context.Dispatch.Fetch<IList<T>>(queryName, null);
            }
            else
            {
                return _context.Dispatch.Fetch<IList<T>>(queryName, args.ToArray());
            }
        }

        public IResponse Execute(CommandName commandName, object[] args)
        {
            return _context.Dispatch.Process<T>(commandName, args);
        }

        #endregion

    }
}
