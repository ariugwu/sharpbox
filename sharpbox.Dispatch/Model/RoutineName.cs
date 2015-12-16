namespace sharpbox.Dispatch.Model
{
    using System;

    [Serializable]
    public class RoutineName : EnumPattern
    {

        public static readonly RoutineName Example = new RoutineName("Example");

        public RoutineName(string value)
            : base(value)
        {
            this.RoutineNameId = Guid.NewGuid();
            this.Name = value;
        }

        public RoutineName()
        {
            this.RoutineNameId = Guid.NewGuid();
        }

        public Guid RoutineNameId { get; set; }
        public string Name { get; set; }
    }
}
