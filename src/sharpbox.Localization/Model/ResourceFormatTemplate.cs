using System;

namespace sharpbox.Localization.Model
{
  [Serializable]
  public abstract class ResourceFormatTemplate
  {
    protected abstract string FormatResource(Resource resource, object entity);
  }
}
