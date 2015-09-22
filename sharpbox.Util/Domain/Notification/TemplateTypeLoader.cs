﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace sharpbox.Util.Domain.Notification
{
  public static class TemplateTypeLoader
  {
    // TODO: Figure out if this represents a security risk
    /// <summary>
    /// Designed to get all the classes which have been marked as options for subscription. 
    /// @SEE: http://stackoverflow.com/questions/26733/getting-all-types-that-implement-an-interface
    /// </summary>
    /// <returns>List of all types which implement the 'marker' interface.</returns>
    public static IEnumerable<Type> GetTypesByITemplateTypeUsage()
    {
      //TODO: Ensure that all appropriate types are in the appdomain and that this link isn't necessary: http://stackoverflow.com/questions/2384592/is-there-a-way-to-force-all-referenced-assemblies-to-be-loaded-into-the-app-doma
      //TODO: Consider a extendable helper pattern that allows any library that needs it the ability to supply it's own "GetTemplateTypes"?
      Type type = typeof(ITemplateType);
      IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes())
          .Where(p => type.IsAssignableFrom(p));

      return types;
    }

    /// <summary>
    /// 
    /// @SEE: http://stackoverflow.com/questions/607178/how-enumerate-all-classes-with-custom-class-attribute
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Type> GetTypesWithEmailTemplateAttribute()
    {
      return
      from a in AppDomain.CurrentDomain.GetAssemblies()
      from t in a.GetTypes()
      let attributes = t.GetCustomAttributes(typeof(EmailTemplateAttribute), true)
      where attributes != null && attributes.Length > 0
      select t;

    }
  }
}