using System.Net.Mail;
using sharpbox.Io.Strategy.Binary;

namespace sharpbox.Bootstrap.Models
{
    public class ExampleAppContext : AppContext
    {
      public ExampleAppContext() : base(new SmtpClient(), new BinaryStrategy()) { }
    }
}
