using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.Util.Dispatch
{
    public static class MetaLoader
    {
        public static string[] AvailableCommands()
        {
            return GetStaticFieldsByType(typeof (CommandName));
        }

        public static string[] AvailableEvents()
        {
            return GetStaticFieldsByType(typeof(CommandName));
        }

        public static string[] AvailableRoutines()
        {
            return GetStaticFieldsByType(typeof(CommandName));
        }

        public static string[] AvailableUiActions()
        {
            return GetStaticFieldsByType(typeof(CommandName));
        }

        private static string[] GetStaticFieldsByType(Type type)
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));

            foreach (var t in types)
            {
                return t.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Select(x => x.Name).ToArray();
            }
        }
    }
}
