using System.Collections.Generic;
using System.Web.Mvc;
using sharpbox.Localization.Model;

namespace sharpbox.Web.Sharpbox.Web.Helpers
{
  public static class Lookup
  {
    public static IList<Environment> Environments(this HtmlHelper helper)
    {
      return AppInfoRepository.Environments();
    }

  }
}