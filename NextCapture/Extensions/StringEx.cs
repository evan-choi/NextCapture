using System;
using System.Linq;

namespace NextCapture
{
    public static class StringEx
    {
        public static bool AnyEquals(this string expression, string obj)
        {
            return expression.Equals(obj, StringComparison.OrdinalIgnoreCase);
        }

        public static TEnum? ToEnum<TEnum>(this string expression) where TEnum : struct
        {
            var type = typeof(TEnum);

            if (type.IsEnum)
            {
                var q = type.GetEnumValues()
                    .Cast<TEnum>()
                    .Where(e => e.ToString().AnyEquals(expression));

                if (q.Count() > 0)
                    return q.First();
            }

            return null;
        }
    }
}
