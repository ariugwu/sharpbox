namespace sharpbox.Membership
{
    using Model;

    public abstract class IdentityStrategy
    {
        public abstract UserStore GetUserStore();

        public abstract RoleStore GetRoleStore();

        public abstract UserClaimStore GetClaimStore();
    }
}
