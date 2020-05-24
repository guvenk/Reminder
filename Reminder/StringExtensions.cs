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
            string count = parts.First().Replace(ContentSeperator, string.Empty);
            var remainingViewCount = int.Parse(count);
            var lastViewed = DateTime.Parse(parts.Skip(1).First());
            var content = parts.Skip(2).First();

            return (remainingViewCount, lastViewed, content);
        }

        public static List<string> SplitContents(this string value)
        {
            return value.Split(ContentSeperator)
                        .Where(a => !string.IsNullOrWhiteSpace(a))
                        .ToList();
        }
    }
}
