using System;
using System.Collections;
using System.Reflection;

namespace ODiff.Extensions
{
    public static class ObjectExtensions
    {
        public static FieldInfo[] PublicFields(this object obj)
        {
            return obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        }

        public static PropertyInfo[] PublicGetterProperties(this object obj)
        {
            return obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
        }

        public static bool IsList(this object obj)
        {
            return obj is IList;
        }

        public static bool IsEnumerable(this object obj)
        {
            return obj is IEnumerable;
        }

        public static bool IsValueType(this object obj)
        {
            return obj != null && (obj.GetType().IsPrimitive || obj.GetType() == typeof(decimal) || obj.GetType() == typeof(DateTime) || obj is string);
        }

        public static bool IsEnum(this object obj)
        {
            return obj != null && obj.GetType().IsEnum;
        }

        public static bool IsAValue(this object obj)
        {
            return obj.IsValueType() || obj.IsEnum();
        }
    }
}
