using System;
using System.Collections;
using System.Reflection;

namespace ODiff.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static Object GetValue(this PropertyInfo property, Object obj)
        {
            return property.GetValue(obj, new object[] {});
        }

        public static bool IsIndexerProperty(this PropertyInfo property)
        {
            return property.GetIndexParameters().Length > 0;
        }
    
        public static bool IsList(this PropertyInfo property)
        {
            return property.GetType() is IEnumerable;
        }

        public static bool Exists(this PropertyInfo property)
        {
            return property is PropertyInfo;
        }
    }
}
