using System;
using System.Collections.Generic;
using System.Linq;

namespace GameWish.Game
{
    public static class EnumUtil
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T ConvertStringToEnum<T>(string value)
        {
            T result = (T)Enum.Parse(typeof(T), value);
            return result;
        }
    }
}

