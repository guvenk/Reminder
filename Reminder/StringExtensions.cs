using System;
using System.Collections.Generic;
using System.Linq;

namespace Reminder
{
    public static class StringExtensions
    {
        private const string ContentSeperator = "@@";
        private const string DbSeparator = "|||";

        public static string ConstructNewContent(this string content, int remindCount)
        {
            string str = $"{ContentSeperator}{remindCount}{DbSeparator}{DateTime.Now}{DbSeparator}{content}";
            return str;
        }

        public static (int RemainingViewCount, DateTime LastViewed, string Content) DeconstructDbContent(this string item)
        {
            var parts = item.Split("|||");
            string count = GetCount(parts);
            var remainingViewCount = int.Parse(count);
            var lastViewed = GetLastViewed(parts);
            var content = GetContent(parts);

            return (remainingViewCount, lastViewed, content);
        }

        private static string GetContent(string[] parts)
        {
            return parts.Skip(2).First();
        }

        private static DateTime GetLastViewed(string[] parts)
        {
            return DateTime.Parse(parts.Skip(1).First());
        }

        private static string GetCount(string[] parts)
        {
            return parts.First().Replace(ContentSeperator, string.Empty);
        }

        public static List<string> SplitContents(this string value)
        {
            return value.Split(ContentSeperator)
                        .Where(a => !string.IsNullOrWhiteSpace(a))
                        .ToList();
        }
    }
}
