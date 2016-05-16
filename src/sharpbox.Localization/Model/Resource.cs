using System;

namespace sharpbox.Localization.Model
{
    [Serializable]
    public class Resource
    {
        public Resource()
        {
            this.ResourceId = Guid.NewGuid();
        }

        public Resource(ResourceName name, ResourceType type, string value, string cultureCode, Guid? environmentId)
        {
            this.ResourceId = Guid.NewGuid();
            this.ResourceName = name;
            this.ResourceType = type;
            this.Value = value;
            this.CreatedDate = DateTime.Now;
            this.LastModifiedDateTime = DateTime.Now;

            this.CultureCode = cultureCode;
            this.EnvironmentId = environmentId;
        }

        public Guid? ResourceId { get; set; }
        public ResourceName ResourceName { get; set; }
        public ResourceType ResourceType { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        /// <summary>
        /// Taken from CultureInfo (Thread.CurrentThread.CurrentCulture.Name)
        /// @SEE: https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.name(v=vs.110).aspx
        /// @SEE: http://www.csharp-examples.net/culture-names/
        /// </summary>
        public string CultureCode { get; set; }

        public Guid? EnvironmentId { get; set; }
    }
}
