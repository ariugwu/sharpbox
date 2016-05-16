using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.Common.Type
{
    using System.Reflection;

    using Type = System.Type;

    public class TypeInfoHelper
    {
        public static PropertyInfo GetIdPropertyByConvention(Type type)
        {
            string idName = $"{type.Name}Id";

            return type.GetProperty(idName);
        }

        public static object GetIdValueByConvention(object thing, Type type)
        {
            var propInfo = GetIdPropertyByConvention(type);

            return propInfo.GetValue(thing);
        }

        public static PropertyInfo GetNamePropertyByConvention(Type type)
        {
            var name = "Name";

            return type.GetProperty(name);
        }

        public static object GetNameValueByConvention(object thing, Type type)
        {
            var propInfo = GetNamePropertyByConvention(type);

            return propInfo.GetValue(thing);
        }
    }
}
