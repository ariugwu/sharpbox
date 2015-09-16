namespace sharpbox.WebLibrary.Core
{
  public class EnvironmentType : sharpbox.Util.Enum.EnumPattern
  {
    public static EnvironmentType Dev = new EnvironmentType("Development");
    public static EnvironmentType Test = new EnvironmentType("Test");
    public static EnvironmentType Production = new EnvironmentType("Production");

    public EnvironmentType(string value) : base(value)
    {
      Name = value;
    }

    public EnvironmentType()
    {
      
    }
    public int EnvironmentTypeId { get; set; }

    public string Name { get; set; }


  }
}
