using Microsoft.AspNet.Identity;

namespace sharpbox.Membership
{
    public abstract class IdentityContext
    {
        public Common.Membership.IdentityStrategy IdentityStrategy { get; set; }

        public UserManager<Model.User, int> UserManger { get; set; }
        public RoleManager<Model.Role, int> RoleManager { get; set; }
        
    }
}
