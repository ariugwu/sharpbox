using System;
using System.Collections.Generic;
using System.Linq;
using sharpbox.Common.Dispatch.Model;
using sharpbox.WebLibrary.Web.Helpers.TypeScript;

namespace sharpbox.WebLibrary.Helpers.TypeScript
{
    using Type = System.Type;

    public static class MetaLoader
    {
        private static IList<Type> _metaStores = GetMetaStoresByMetaLoaderAttribute();

        public static string[] AvailableCommands()
        {
            return GetFieldNamesByType(_metaStores, typeof(CommandName));
        }

        public static string[] AvailableEvents()
        {
            return GetFieldNamesByType(_metaStores, typeof(EventName));
        }

        public static string[] AvailableRoutines()
        {
            return GetFieldNamesByType(_metaStores, typeof(RoutineName));
        }

        public static string[] AvailableUiActions()
        {
            return GetFieldNamesByType(_metaStores, typeof(CommandName));
        }

        private static string[] GetFieldNamesByType(IEnumerable<Type> types, Type targetType)
        {
            return types.SelectMany(x => x.GetFields()).Where(y => y.FieldType == targetType).Select(y => y.Name).ToArray();
        }

        private static IList<Type> GetMetaStoresByMetaLoaderAttribute()
        {
           return
            (from a in AppDomain.CurrentDomain.GetAssemblies()
            from t in a.GetTypes()
            let attributes = t.GetCustomAttributes(typeof(GenerateTypeScriptMetadata), true)
            where attributes != null && attributes.Length > 0
            select t).ToList();
        }

        public static IList<Type> GetAllMetaDataClasses()
        {
            Type type = typeof(IDispatchMetadata);
            IList<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p)).ToList();

            return types;

        }

    }
}
