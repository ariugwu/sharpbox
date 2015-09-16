using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using sharpbox.Dispatch.Model;
using sharpbox.EfCodeFirst.Audit;

namespace sharpbox.Bootstrap.Package.Mvc.Controllers
{
    /*
    To add a route for this controller, merge these statements into the Register method of the WebApiConfig class. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using sharpbox.Dispatch.Model;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Response>("SharpboxApi");
    builder.EntitySet<EventName>("EventNames"); 
    builder.EntitySet<Request>("Requests"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SharpboxApiController<T> : ODataController
    {
        private AuditContext db = new AuditContext();

        // GET odata/SharpboxApi
        [Queryable]
        public IQueryable<Response> GetSharpboxApi()
        {
            return db.Responses;
        }

        // GET odata/SharpboxApi(5)
        [Queryable]
        public SingleResult<T> Get([FromODataUri] int key)
        {
            return SingleResult.Create(db.Responses.Where(response => response.ResponseId == key));
        }

        // PUT odata/SharpboxApi(5)
        public IHttpActionResult Put([FromODataUri] int key, T instance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != response.ResponseId)
            {
                return BadRequest();
            }

            db.Entry(response).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponseExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(response);
        }

        // POST odata/SharpboxApi
        public IHttpActionResult Post(Response response)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Responses.Add(response);
            db.SaveChanges();

            return Created(response);
        }

        // PATCH odata/SharpboxApi(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Response> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Response response = db.Responses.Find(key);
            if (response == null)
            {
                return NotFound();
            }

            patch.Patch(response);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponseExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(response);
        }

        // DELETE odata/SharpboxApi(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Response response = db.Responses.Find(key);
            if (response == null)
            {
                return NotFound();
            }

            db.Responses.Remove(response);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/SharpboxApi(5)/EventName
        [Queryable]
        public SingleResult<EventName> GetEventName([FromODataUri] int key)
        {
            return SingleResult.Create(db.Responses.Where(m => m.ResponseId == key).Select(m => m.EventName));
        }

        // GET odata/SharpboxApi(5)/Request
        [Queryable]
        public SingleResult<Request> GetRequest([FromODataUri] int key)
        {
            return SingleResult.Create(db.Responses.Where(m => m.ResponseId == key).Select(m => m.Request));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ResponseExists(int key)
        {
            return db.Responses.Count(e => e.ResponseId == key) > 0;
        }
    }
}
