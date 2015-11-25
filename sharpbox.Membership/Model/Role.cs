using System;

namespace sharpbox.Membership.Model
{
    using Common.Type;

    [Serializable]
    public class Role : EnumPattern, Microsoft.AspNet.Identity.IRole<int>
    {
        public Role(string value)
          : base(value)
        {
            Name = value;
        }

        public int Id { get; }

        public string Name { get; set; }

        public static Role Administrator = new Role("Administrator");
        public static Role User = new Role("User");

        public Guid EnvironmentId { get; set; }
    }
}
