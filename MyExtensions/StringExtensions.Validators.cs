using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensionsValidators
    {
        #region [IsAlpha]
        private static Regex IsAlphaRegex = new Regex(RegexPattern.ALPHA, RegexOptions.Compiled);

        /// <summary>
        /// Determines whether the specified eval string contains only alpha characters.
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified eval string is alpha; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlpha(this string evalString)
        {
            return IsAlphaRegex.IsMatch(evalString);
        }
        #endregion

        #region [IsAlphaNumeric]
        private static Regex IsAlphaNumericRegex = new Regex(RegexPattern.ALPHA_NUMERIC, RegexOptions.Compiled);

        /// <summary>
        /// Determines whether the specified eval string contains only alphanumeric characters
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        /// 	<c>true</c> if the string is alphanumeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphaNumeric(this string evalString)
        {
            return IsAlphaNumericRegex.IsMatch(evalString);
        }
        #endregion

        #region [IsNumeric]
        private static Regex isNumericRegex = new Regex(RegexPattern.NUMERIC, RegexOptions.Compiled);

        public static bool IsNumeric(string inputString)
        {
            Match m = isNumericRegex.Match(inputString);
            return m.Success;
        } 
        #endregion

        #region IsAbsolutePhysicalPath
        private static bool IsDirectorySeparatorChar(char ch)
        {
            if (ch != '\\')
            {
                return (ch == '/');
            }
            return true;
        }

        internal static bool IsUncSharePath(string path)
        {
            return (((path.Length > 2) && IsDirectorySeparatorChar(path[0])) && IsDirectorySeparatorChar(path[1]));
        }

        public static bool IsAbsolutePhysicalPath(this string path)
        {
            if ((path == null) || (path.Length < 3))
            {
                return false;
            }
            return (((path[1] == ':') && IsDirectorySeparatorChar(path[2])) || IsUncSharePath(path));
        }
        #endregion

        #region IsAppRelativePath
        public static bool IsAppRelativePath(this string path)
        {
            if (path == null)
            {
                return false;
            }
            int length = path.Length;
            if (length == 0)
            {
                return false;
            }
            if (path[0] != '~')
            {
                return false;
            }
            if ((length != 1) && (path[1] != '\\'))
            {
                return (path[1] == '/');
            }
            return true;
        }
        #endregion

        #region [IsEmailAddress]

        private static Regex isValidEmailRegex = new Regex(RegexPattern.EMAIL, RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEmailAddress(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            return isValidEmailRegex.IsMatch(s);
        }

        #endregion

        #region [IsGuid]
        private static Regex isGuidRegex = new Regex(RegexPattern.GUID, RegexOptions.Compiled);

        public static bool IsGuid(this string candidate)
        {
            bool isValid = false;

            if (!string.IsNullOrEmpty(candidate))
            {
                isValid = isGuidRegex.IsMatch(candidate);
            }
            return isValid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public static bool IsGuid(this string candidate, out Guid output)
        {
            bool isValid = false;
            output = Guid.Empty;
            if (candidate.IsGuid())
            {
                isValid = true;
                output = new Guid(candidate);
            }
            return isValid;
        }
        #endregion

        #region [IsIPAddress]
        private static Regex isIPAddressRegex = new Regex(RegexPattern.IP_ADDRESS, RegexOptions.Compiled);

        /// <summary>
        /// Determines whether the specified string is a valid IP address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>
        /// 	<c>true</c> if valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIPAddress(this string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return false;
            return isIPAddressRegex.IsMatch(ipAddress);
        }
        #endregion

        #region [IsLowerCase]

        private static Regex isLowerCaseRegex = new Regex(RegexPattern.LOWER_CASE, RegexOptions.Compiled);

        /// <summary>
        /// Determines whether the specified string is lower case.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is lower case; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLowerCase(this string inputString)
        {
            return isLowerCaseRegex.IsMatch(inputString);
        }
        #endregion

        #region [IsUpperCase]
        private static Regex isUpperRegex = new Regex(RegexPattern.UPPER_CASE, RegexOptions.Compiled);
        /// <summary>
        /// Determines whether the specified string is upper case.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is upper case; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUpperCase(this string inputString)
        {
            return isUpperRegex.IsMatch(inputString);
        }
        #endregion

        #region [IsUrl]
        private static Regex isUrlRegex = new Regex(RegexPattern.URL, RegexOptions.Compiled);
        /// <summary>
        /// Determines whether the specified string is url.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>
        /// 	<c>true</c> if the specified string is url; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUrl(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return false;
            return isUrlRegex.IsMatch(inputString);
        }
        #endregion

        #region [IsHasChinese]
        private static Regex isHasChineseRegex = new Regex(RegexPattern.HasCHINESE, RegexOptions.Compiled);

        public static bool IsHasChinese(string inputString)
        {
            Match m = isHasChineseRegex.Match(inputString);
            return m.Success;
        }
        #endregion
    }
}
