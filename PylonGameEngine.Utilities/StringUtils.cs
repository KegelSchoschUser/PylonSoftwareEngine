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
    }
}
