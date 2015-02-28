using sharpbox.Localization.Model;

namespace sharpbox.Cli.Model.Domain.Localization
{
    public class ResourceNamesExt : Util.Enum.EnumPattern
    {
        public static ResourceNames ProjectTitle = new ResourceNames("ProjectTitle");
        public static ResourceNames FooterText = new ResourceNames("FooterText");
        public static ResourceNames ImpersonationHelpText = new ResourceNames("ImpersonationHelpText");
        public static ResourceNames ServerErrorHelpText = new ResourceNames("ServerErrorHelpText");
        public static ResourceNames PageNotFoundErrorHelpText = new ResourceNames("PageNotFoundHelpText");
    }
}
