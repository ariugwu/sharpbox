using System;

namespace sharpbox.App.Model
{
  [Serializable]
  public class Environment
  {
    public int EnvironmentId { get; set; }
    public string BaseUrl { get; set; }
    public string CacheKey { get; set; }
    public string ApplicationName { get; set; }
    public string UploadDirectory { get; set; }
    public string LogoLocation { get; set; }
    public int BrandTypeId { get; set; }
    public BrandType BrandType { get; set; }
    public int EnvironmentTypeId { get; set; }
    public EnvironmentType EnvironmentType {get; set; }
    public int TechSheetId { get; set; }
    public TechSheet TechSheet { get; set; }
    public Guid? ApplicationId { get; set; }
  }
}
