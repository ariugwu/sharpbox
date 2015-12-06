namespace sharpbox.Membership.Model
{
    using System;

    public class User : Microsoft.AspNet.Identity.IUser<Guid>
    {
        public Guid Id { get; }
        public string UserName { get; set; }
    }
}
