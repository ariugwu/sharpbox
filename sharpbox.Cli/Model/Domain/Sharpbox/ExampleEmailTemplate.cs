using System;
using sharpbox.Localization.Model;

namespace sharpbox.Cli.Model.Domain.Sharpbox
{
  [Serializable]
  public class ExampleEmailTemplate : EmailTemplate
  {
    public ExampleEmailTemplate(Type entityType, Resource subjectResource, Resource bodyResource) : base(entityType, subjectResource, bodyResource)
    {
    }

    protected override string FormatSubject(Resource resource, object entity)
    {
      return string.Format(resource.Value, entity);
    }

    protected override string FormatBody(Resource resource, object entity)
    {
      return string.Format(resource.Value, entity);
    }
  }
}
