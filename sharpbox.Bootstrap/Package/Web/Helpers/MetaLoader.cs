using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using sharpbox.Common.Dispatch.Model;

namespace sharpbox.WebLibrary.Helpers
{
    public static class MetaLoader
    {
        public static string[] AvailableCommands()
        {
            return GetStaticFieldsByType(typeof (CommandName));
        }

        public static string[] AvailableEvents()
        {
            return GetStaticFieldsByType(typeof(EventName));
        }

        public static string[] AvailableRoutines()
        {
            return GetStaticFieldsByType(typeof(RoutineName));
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

            return types.SelectMany(x => x.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)).Select(y => y.Name).ToArray();

        }

        private static string[] GetSharpboxControllers(Type type)
        {
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));

            return types.SelectMany(x => x.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)).Select(y => y.Name).ToArray();

        }
    }
}
