using sharpbox.Membership.Model;
using System;
using System.Collections.Generic;
using sharpbox.Dispatch.Model;

namespace sharpbox.Localization.Model
{
  [Serializable]
  public abstract class EmailTemplate : ResourceFormatTemplate
  {
    protected EmailTemplate(Type entityType, Resource subjectResource, Resource bodyResource)
    {
      this.EntityType = entityType;
      this._subjectResource = subjectResource;
      this._bodyResource = bodyResource;
    }

    private readonly Resource _subjectResource;
    private readonly Resource _bodyResource;

    public int EmailTemplateId { get; set; }

    public Dictionary<EventName, Dictionary<Type, UserRole>> SubscribedRoles { get; set; }

    public Type EntityType { get; private set; }

    public string GetSubject(object entity)
    {
      return this.FormatResource(this._subjectResource, entity);
    }

    public string GetBody(object entity)
    {
      return this.FormatResource(this._bodyResource, entity);
    }

    protected override string FormatResource(Resource resource, object entity)
    {
      if (resource.ResourceType.Equals(ResourceType.EmailSubject)) return this.FormatSubject(resource, entity);
      if (resource.ResourceType.Equals(ResourceType.EmailBody)) return this.FormatBody(resource, entity);

      throw new ArgumentException("Resource Type not supported by this template");
    }

    protected abstract string FormatSubject(Resource resource, object entity);
    protected abstract string FormatBody(Resource resource, object entity);

  }
}
