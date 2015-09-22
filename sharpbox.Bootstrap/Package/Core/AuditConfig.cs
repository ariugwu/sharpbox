using System;
using System.ComponentModel.DataAnnotations;

namespace sharpbox.WebLibrary.Core.App.Info.Model
{
  public class AuditConfig
  {
    [Key]
    public int AuditConfigId { get; set; }

    /// <summary>
    /// For use by the base strategy which stores information in an XML file.
    /// </summary>
    [MaxLength(255)]
    public string AuditFailOverFileLocation { get; set; }

    public Guid? ApplicationId { get; set; }
  }
}
