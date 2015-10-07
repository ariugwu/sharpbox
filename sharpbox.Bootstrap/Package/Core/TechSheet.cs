using System;
using System.ComponentModel.DataAnnotations;

namespace sharpbox.WebLibrary.Core.App.Info.Model
{
  public class TechSheet
  {
    [Key]
    public int TechSheetId { get; set; }
    public string WebServer { get; set; }
    public string DataSource { get; set; }
    public string Database { get; set; }
    public int TfsConfigId { get; set; }
    public TfsConfig TfsConfig { get; set; }
    public Guid? ApplicationId { get; set; }
  }
}
