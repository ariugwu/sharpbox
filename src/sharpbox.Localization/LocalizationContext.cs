using System.Collections.Generic;

namespace sharpbox.Localization
{
    using Model;

    public class LocalizationContext
    {
        public LocalizationContext(string cultureCode)
        {
            this.CultureCode = cultureCode;
        }

        public string CultureCode { get; set; }

        public Dictionary<ResourceName, string> Resources { get; set; }
    }
}
