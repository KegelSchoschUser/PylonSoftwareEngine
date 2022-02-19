using System;

namespace PylonGameEngine.Utilities
{
    public static class StringUtilities
    {
        public static string SafeSubString(string s, int length)
        {
            string Output = "";
            Output = SafeSubString(s, 0, length);

            return Output;
        }

        public static string SafeSubString(string s, int startIndex, int length)
        {
            string Output = "";
            if (s.Length - startIndex >= length)
            {
                Output = s.Substring(startIndex, length);
            }
            else
            {
                Output = s.Substring(startIndex, s.Length - startIndex);
            }


            return Output;
        }

        public static string GetStringTillClosingChar(string s, char ClosingChar)
        {
            string Output = null;
            if (s.Contains(ClosingChar))
            {
                Output = s.Split(ClosingChar)[0];
            }

            return Output;
        }

        public static string GetTimeSpanFormatedText(float inputseconds, bool ShowMilliseconds = true, string specificFormat = null)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(inputseconds);

            if (specificFormat != null)
                return timeSpan.ToString(specificFormat);
            string Format = "";
            if (timeSpan.TotalDays > 1)
                Format += @"dd\:";
            if (timeSpan.TotalHours > 1)
                Format += @"hh\:";
            if (timeSpan.TotalMinutes > 1)
                Format += @"mm\:";
            if (timeSpan.TotalSeconds > 1)
                Format += @"ss";

            if (ShowMilliseconds)
                Format += @"\.ffffff";
            return timeSpan.ToString(Format);
        }
    }
}
