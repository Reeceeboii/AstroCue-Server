namespace AstroCue.Server.Utilities
{
    /// <summary>
    /// Class for various string helper functions
    /// </summary>
    public class StringUtilities
    {
        /// <summary>
        /// Given a string, trim excess whitespace and make the first character uppercase
        /// </summary>
        /// <example>"   john       " becomes "John"</example>
        /// <param name="s">The string to be operated on</param>
        /// <returns>The string with char[0] uppercased and excess whitespace removed</returns>
        public static string TrimToUpperFirstChar(string s)
        {
            s = s.Trim();
            return char.ToUpper(s[0]) + s[1..];
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
