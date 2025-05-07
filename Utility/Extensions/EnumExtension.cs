using System;

namespace Chipmunk.Library.Utility.Extensions
{
    public static class EnumExtension
    {
        public static T GetNext<T>(this T enumValue) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            int length = values.Length;

            if (length <= 1)
                return enumValue;

            int currentIndex = Array.IndexOf(values, enumValue);

            return (0 <= currentIndex && currentIndex < length - 1)
                ? (T)values.GetValue(currentIndex + 1)
                : (T)values.GetValue(0);
        }

        public static T GetPrev<T>(this T enumValue) where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            int length = values.Length;

            if (length <= 1)
                return enumValue;

            int currentIndex = Array.IndexOf(values, enumValue);

            return (currentIndex > 0)
                ? (T)values.GetValue(currentIndex - 1)
                : (T)values.GetValue(length - 1);
        }

        public static T GetRandom<T>() where T : Enum
        {
            Array array = Enum.GetValues(typeof(T));
            System.Random random = new System.Random();
            return (T)array.GetValue(random.Next(array.Length));
        }

        public static bool IsFirst<T>(this T enumValue) where T : Enum
        {
            Array array = Enum.GetValues(typeof(T));
            return enumValue.Equals(array.GetValue(0));
        }

        public static bool IsLast<T>(this T enumValue) where T : Enum
        {
            Array array = Enum.GetValues(typeof(T));
            return enumValue.Equals(array.GetValue(array.Length - 1));
        }
    }
}