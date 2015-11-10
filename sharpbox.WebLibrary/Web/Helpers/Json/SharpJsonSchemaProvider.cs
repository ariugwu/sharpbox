namespace sharpbox.WebLibrary.Web.Helpers.Json
{
    using System;
    using System.ComponentModel;

    using Newtonsoft.Json.Schema;
    using Newtonsoft.Json.Schema.Generation;

    public class SharpJsonSchemaProvider<T> : JSchemaGenerationProvider where T: new()
    {
        public SharpJsonSchemaProvider(TypeDescriptionProvider tdp)
        {
            _typeDescriptionProvidor = tdp;
        }

        private TypeDescriptionProvider _typeDescriptionProvidor;

        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var instance = new T();

            var schema = new JSchema()
                             {
                                 Type = this.GetJSchemaType(context.ObjectType)
                             };

            schema.Format = "string";
            schema.Title = "";
            schema.MaximumLength = 10;
            return schema;
        }

        private JSchemaType GetJSchemaType(Type type)
        {
            if (IsNumber(type))
            {
                return JSchemaType.Number;
            }

            if (IsBoolean(type))
            {
                return JSchemaType.Boolean;
            }

            return JSchemaType.Object;
        }

        private static bool IsNumber(Type type)
        {
            return (type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong)
                    || type == typeof(short) || type == typeof(ushort));
        }

        private static bool IsBoolean(Type type)
        {
            return (type == typeof(bool));
        }
    }
}
