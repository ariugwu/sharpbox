using System;

namespace sharpbox.Localization.Model
{
    using Common.Data;

    [Serializable]
    public class Resource: ISharpThing<Resource>
    {
        public Resource() { }

        public Resource(ResourceName name, ResourceType type, string value, string cultureCode, Guid? applicationSharpId)
        {
            this.SharpId = Guid.NewGuid();
            this.ResourceName = name;
            this.ResourceType = type;
            this.Value = value;
            this.CreatedDate = DateTime.Now;
            this.LastModifiedDateTime = DateTime.Now;

            this.CultureCode = cultureCode;
            this.ApplicationSharpId = applicationSharpId;
        }

        public Guid SharpId { get; set; }

        public int ResourceId { get; set; }
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

        public Guid? ApplicationSharpId { get; set; }
    }
}
