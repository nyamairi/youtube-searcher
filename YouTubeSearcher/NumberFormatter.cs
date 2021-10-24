using System;

namespace YouTubeSearcher
{
    internal static class NumberFormatter
    {
        private static readonly string[] Suffixes = { string.Empty, "k", "m", "g" };

        public static string ToSummarizedString(ulong value)
        {
            var d = (double)value;
            var i = 0;
            while (d / 1000.0d >= 1.0d && i < Suffixes.Length - 1)
            {
                d /= 1000.0d;
                i++;
            }

            var suffix = Suffixes[i];
            if (string.IsNullOrEmpty(suffix))
            {
                return value.ToString();
            }

            return Math.Round(d, 1).ToString("#,###.0") + suffix;
        }
    }
}