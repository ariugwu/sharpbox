using System;
using System.ComponentModel.DataAnnotations;

namespace sharpbox.WebLibrary.Core.App.Info.Model
{
  public class TfsConfig
  {
    [Key]
   public int TfsConfigId { get; set; }
    [MaxLength(255)]
    public string Server { get; set; }
    [MaxLength(255)]
    public string ProjectName { get; set; }

    public string ProjectSiteUrl { get; set; }

    public string SourceLocation { get; set; }

    public Guid? ApplicationId { get; set; }
  }
}
