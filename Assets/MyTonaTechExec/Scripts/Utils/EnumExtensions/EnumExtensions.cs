using System;

namespace MyTonaTechExec.Utils.EnumExtensions
{
    public static class EnumExtensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");

            var array = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf(array, src) + 1;
            return array.Length == j ? array[0] : array[j];
        }
    }
}