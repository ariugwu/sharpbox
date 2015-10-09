using System;
using System.ComponentModel.DataAnnotations;

namespace sharpbox.WebLibrary.Core
{
  public class TechSheet
  {
    [Key]
    public int TechSheetId { get; set; }
    public string WebServer { get; set; }
    public string DataSource { get; set; }
    public string Database { get; set; }
    public int SourceControlId { get; set; }
    public SourceControl SourceControl { get; set; }
    public Guid? ApplicationId { get; set; }
  }
}
