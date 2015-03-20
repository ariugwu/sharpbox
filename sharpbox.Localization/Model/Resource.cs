using System;
namespace sharpbox.Localization.Model
{
    [Serializable]
    public class Resource
    {
        public Resource() { }

        public int ResourceId { get; set; }
        public ResourceName ResourceName { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        /// <summary>
        /// Taken from CultureInfo (Thread.CurrentThread.CurrentCulture.Name)
        /// @SEE: https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.name(v=vs.110).aspx
        /// @SEE: http://www.csharp-examples.net/culture-names/
        /// </summary>
        public string CultureCode { get; set; }

    }
}
