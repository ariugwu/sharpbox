using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpbox.WebLibrary.Web.Helpers.Json
{
    using System.Reflection;

    public class JsonSchemaGenerator
    {
        public static string Generate<T>()
        {
            Type type = typeof(T);
            var jsonSchema = "{ \"type\": \"object\",";
                jsonSchema += "}";
                
            return jsonSchema;
        }

        private static string BuildObject(Type type)
        {
            var jsonSchema = "{ \"type\": \"object\"";
                    jsonSchema += BuildProperties(type);
                jsonSchema += "}";

            return jsonSchema;
        }

        private static string BuildProperties(Type type)
        {
            var properties = "\"properties\": {";
            //TODO: if a property is an object, recurse.
            // This will only use public properties. Is that enough?
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    properties += FillProp(propertyInfo) + ",";
                }
            }
            properties += "}";

            return properties;
        }

        #region Helper(s)
        private static string FillProp(PropertyInfo propertyInfo)
        {
            string filledProp = "";

            Type propType = propertyInfo.PropertyType;
            string name = propertyInfo.Name;
            string type;
            string format;
            //int minValue;
            //int maxValue;
            //string isChildArray;
            bool isLookup = false;

            if (IsNumber(propType))
            {
                type = format = "number";
            }

            if (IsBoolean(propType))
            {
                type = format = "boolean";
            }

            if (IsDateTime(propType))
            {
                type = format = "date-time";
            }

            type = format = "string";

            filledProp = $"\"{name}\": {{ \"type\": \"{type}\", \"format\": \"{format}\", \"isLookup\": \"{isLookup}\"}}";

            return filledProp;
        }

        private static bool IsNumber(Type type)
        {
            return type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong)
                    || type == typeof(short) || type == typeof(ushort);
        }

        private static bool IsBoolean(Type type)
        {
            return type == typeof(bool);
        }

        private static bool IsDateTime(Type type)
        {
            return type == typeof(DateTime) || type == typeof(DateTimeOffset);
        }

        /// <summary>
        /// @SEE: http://stackoverflow.com/questions/374651/how-to-check-if-an-object-is-nullable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsNullable(Type type)
        {
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }

        #endregion
    }
}
