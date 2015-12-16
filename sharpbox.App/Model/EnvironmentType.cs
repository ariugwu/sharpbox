using System;

namespace sharpbox.App.Model
{
    using Common.Type;

    [Serializable]
    public class EnvironmentType : EnumPattern
  {
    public static EnvironmentType Dev = new EnvironmentType("Development");
    public static EnvironmentType Integration = new EnvironmentType("Integration");
    public static EnvironmentType Staging = new EnvironmentType("Staging");
    public static EnvironmentType Production = new EnvironmentType("Production");

    public EnvironmentType(string value) : base(value)
    {
      this.Name = value;
    }

    public EnvironmentType()
    {
      
    }
    public Guid EnvironmentTypeId { get; set; }

    public string Name { get; set; }
  }
}
