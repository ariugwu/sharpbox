using System;

namespace sharpbox.App.Model
{
    [Serializable]
    public class SourceControl
    {
        public int SourceControlId { get; set; }
        public string ServerAddress { get; set; }
        public string ProjectName { get; set; }
        public string ProjectSiteUrl { get; set; }
        public string SourceLocation { get; set; }
        public Guid? ApplicationId { get; set; }
    }
}
