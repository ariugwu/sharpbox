using System;

namespace sharpbox.Membership.Model
{
    using Common.Dispatch.Model;

    [Serializable]
    public class Claim
    {
        public int ClaimId { get; set; }
        public Type EntityType { get; set; }
        public EntityStateName EntityState { get; set; }
        public CommandName CommandName { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
