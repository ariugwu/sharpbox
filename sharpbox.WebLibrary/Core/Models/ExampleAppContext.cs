using System.Net.Mail;
using sharpbox.App;
using sharpbox.Io.Strategy.Binary;

namespace sharpbox.WebLibrary.Core.Models
{
    public class ExampleAppContext : AppContext
    {
      public ExampleAppContext() : base("en-us", new SmtpClient(), new BinaryStrategy()) { }
    }
}
