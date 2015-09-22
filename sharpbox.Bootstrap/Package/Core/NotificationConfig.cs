using System;
using System.ComponentModel.DataAnnotations;

namespace sharpbox.WebLibrary.Core.App.Info.Model
{
  public class NotificationConfig
  {
    public int NotificationConfigId { get; set; }

    [MaxLength(255)]
    public string NotificationFailOverFileLocation { get; set; }

    public Guid? ApplicationId { get; set; }
  }
}
