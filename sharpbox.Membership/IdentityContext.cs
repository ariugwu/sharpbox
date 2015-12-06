using Microsoft.AspNet.Identity;

namespace sharpbox.Membership
{
    using System;

    using Model;

    public abstract class IdentityContext
    {
        public IdentityContext(IdentityStrategy identityStrategy)
        {
            this.IdentityStrategy = identityStrategy;
            this.UserManger = new UserManager<User, Guid>(this.IdentityStrategy.GetUserStore());
            this.RoleManager = new RoleManager<Role, Guid>(this.IdentityStrategy.GetRoleStore());
            this.UserClaimStore = this.IdentityStrategy.GetClaimStore();
        }

        public IdentityStrategy IdentityStrategy { get; set; }

        public UserManager<User, Guid> UserManger { get; set; }
        public RoleManager<Role, Guid> RoleManager { get; set; }
        public IUserClaimStore<User,Guid> UserClaimStore { get; set; } 
        
    }
}
