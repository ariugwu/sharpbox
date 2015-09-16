using System;
using System.ComponentModel.DataAnnotations;

namespace sharpbox.WebLibrary.Core.App.Info.Model
{
  public class Environment
  {
    [Key]
    public int EnvironmentId { get; set; }
    public string BaseUrl { get; set; }
    public string ApplicationName { get; set; }
    public string LogoLocation { get; set; }
    public int BrandTypeId { get; set; }
    public BrandType BrandType { get; set; }
    public int EnvironmentTypeId { get; set; }
    public EnvironmentType EnvironmentType {get; set; }
    public string UploadDirectory { get; set; }
    public int TechSheetId { get; set; }
    public TechSheet TechSheet { get; set; }
    public int ApplicationArchitectureId { get; set; }
    public ApplicationArchitecture ApplicationArchitecture { get; set; }

    public Guid? ApplicationId { get; set; }
  }
}
