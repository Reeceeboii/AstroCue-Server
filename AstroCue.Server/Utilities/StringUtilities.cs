namespace AstroCue.Server.Utilities
{
    using System;

    /// <summary>
    /// Class for testing <see cref="StringUtilities"/> methods
    /// </summary>
    public static class StringUtilities
    {
        /// <summary>
        /// Given a string, trim excess whitespace and make the first character uppercase
        /// </summary>
        /// <example>"   john       " becomes "John"</example>
        /// <param name="s">The string to be operated on</param>
        /// <returns>The string with char[0] uppercased and excess whitespace removed</returns>
        public static string TrimToUpperFirstChar(string s)
        {
            try
            {
                s = s.Trim();
                return char.ToUpper(s[0]) + s[1..];
            }
            catch (IndexOutOfRangeException)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Given a string, trim excess whitespace and make the entire string itself lowercase
        /// </summary>
        /// <example>"  EXAmpLe@example.COM  " becomes "example@example.com"</example>
        /// <param name="s"></param>
        /// <returns>The string, lowercased and trimmed</returns>
        public static string TrimToLowerAll(string s)
        {
            return s.Trim().ToLower();
        }
    }
}
