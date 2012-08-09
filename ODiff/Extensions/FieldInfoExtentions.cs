using System.Reflection;

namespace ODiff.Extensions
{
    public static class FieldInfoExtentions
    {
        public static bool Exists(this FieldInfo property)
        {
            return property is FieldInfo;
        }
    }
}
