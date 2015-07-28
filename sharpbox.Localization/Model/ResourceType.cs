using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.Localization.Model
{
  [Serializable]
  public class ResourceType : Util.Enum.EnumPattern
  {
    public static ResourceType Layout = new ResourceType("Layout");
    public static ResourceType Feedback = new ResourceType("Feedback");
    public static ResourceType EmailSubject = new ResourceType("EmailSubject");
    public static ResourceType EmailBody = new ResourceType("EmailBody");
    public ResourceType(string value): base(value){}
  }
}
