using System;

namespace sharpbox.EfCodeFirst.App
{
    using sharpbox.App.Model;

    public class AppUnitOfWork : Common.Data.IUnitOfWork<Environment>
    {
        public string ConnectionStringName { get; set; }

        public Environment Add(Environment instance)
        {
            throw new NotImplementedException();
        }

        public Environment Update(Environment instance)
        {
            throw new NotImplementedException();
        }

        public Environment Remove(Environment instance)
        {
            throw new NotImplementedException();
        }
    }
}
