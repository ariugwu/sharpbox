﻿using System;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.Membership.Model
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public Type EntityType { get; set; }
        public EntityStateName EntityState { get; set; }
        public CommandName CommandName { get; set; }

        public Guid ApplicationId { get; set; }
    }
}