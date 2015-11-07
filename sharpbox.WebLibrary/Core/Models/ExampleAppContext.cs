namespace sharpbox.WebLibrary.Core.Models
{
    using System.Net.Mail;

    using sharpbox.Io.Strategy.Binary;

    public class ExampleAppContext : AppContext
    {
      public ExampleAppContext() : base(new SmtpClient(), new BinaryStrategy()) { }
    }
}
