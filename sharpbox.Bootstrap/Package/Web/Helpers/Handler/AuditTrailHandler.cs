namespace sharpbox.WebLibrary.Web.Helpers.Handler
{
    using Controllers;
    using Core;
    using EfCodeFirst.Audit;

    public class AuditTrailHandler<T> : LifecycleHandler<T> where T : new()
    {
        public override void HandleRequest(WebContext<T> webContext, ISharpboxController<T> controller)
        {
            // Persist audit respones to a database.
            var auditUnitOfWork = new AuditUnitOfWork(webContext.AppContext.DefaultConnectionStringName);

            foreach (var r in webContext.AppContext.Audit.Trail)
            {
                auditUnitOfWork.Add(r);
            }
        }
    }
}