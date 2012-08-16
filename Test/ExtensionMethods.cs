using System;
using System.Globalization;

namespace Test
{
    public static class ExtensionMethods
    {
        public static string FormataParaSPED(this DateTime value)
        {
            return value.ToString("ddMMyyyy") + "|";
        }

        public static string FormataParaSPED(this DateTime? value)
        {
            if (value.HasValue)
                return value.Value.FormataParaSPED();
            return "".FormataParaSPED();
        }

        public static string FormataParaSPED(this double value)
        {
            return
                value.ToString("N").Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator.ToString(), "")
                    .Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString(), ",") + "|";
        }

        public static string FormataParaSPED(this Enum value)
        {
            return (Convert.ToInt32(value)).FormataParaSPED();
        }

        public static string FormataParaSPED(this int value)
        {
            return value.ToString("D").Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator.ToString(), "") + "|";
        }

        public static string FormataParaSPED(this string value)
        {
            return value + "|";
        }

        public static string FimDaLinha(this string value)
        {
            return value + "\r\n";
        }
    }
}
