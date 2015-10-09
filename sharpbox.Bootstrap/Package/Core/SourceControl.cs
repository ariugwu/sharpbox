using System;
using System.ComponentModel.DataAnnotations;

namespace sharpbox.WebLibrary.Core
{
  public class SourceControl
  {
    [Key]
    public int SourceControlId { get; set; }
    [MaxLength(255)]
    public string ServerAddress { get; set; }
    [MaxLength(255)]
    public string ProjectName { get; set; }
    public string ProjectSiteUrl { get; set; }

    public string SourceLocation { get; set; }

    public Guid? ApplicationId { get; set; }
  }
}
