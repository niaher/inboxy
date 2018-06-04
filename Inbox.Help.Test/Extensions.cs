namespace Inbox.Help.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Extensions
    {
        public static IEnumerable<Type> GetTypesDecoratedByAttribute<T>(this Assembly assembly)
        {
            return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(T), true).Length > 0);
        }
    }
}