namespace sharpbox.Membership.Model
{
    public class UserRoleClaim
    {
        public int UserRoleClaimId { get; set; }
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}
