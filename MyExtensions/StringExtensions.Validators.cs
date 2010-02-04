using System.Text.RegularExpressions;
using System.Text;

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

        public static bool IsHasChinese(this string inputString)
        {
            Match m = isHasChineseRegex.Match(inputString);
            return m.Success;
        }
        #endregion

        #region [IsChineseLetter]
        /// <summary>
        /// 在unicode 字符串中，中文的范围是在4E00..9FFF:CJK Unified Ideographs。通过对字符的unicode编码进行判断来确定字符是否为中文。 
        /// </summary>
        /// <remarks>http://blog.csdn.net/qiujiahao/archive/2007/08/09/1733169.aspx</remarks>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsChineseLetter(this string input, int index)
        {
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);
            if (input != "")
            {
                code = Char.ConvertToUtf32(input, index);    //获得字符串input中指定索引index处字符unicode编码

                if (code >= chfrom && code <= chend)
                {
                    return true;     //当code在中文范围内返回true

                }
                else
                {
                    return false;    //当code不在中文范围内返回false
                }
            }
            return false;
        } //
        #endregion

        #region [IsGBCode]
        /// <summary>
        /// 判断一个word是否为GB2312编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsGBCode(this string word)
        {
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(word);
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code or other code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 176 && byte1 <= 247 && byte2 >= 160 && byte2 <= 254)    //判断是否是GB2312
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        } 
        #endregion

        #region [IsGBKCode]
        /// <summary>
        /// 判断一个word是否为GBK编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsGBKCode(this string word)
        {
            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(word.ToString());
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 129 && byte1 <= 254 && byte2 >= 64 && byte2 <= 254)     //判断是否是GBK编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        } 
        #endregion

        #region [IsBig5Code]
        /// <summary>
        /// 判断一个word是否为GBK编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsBig5Code(this string word)
        {
            byte[] bytes = Encoding.GetEncoding("Big5").GetBytes(word.ToString());
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if ((byte1 >= 129 && byte1 <= 254) && ((byte2 >= 64 && byte2 <= 126) || (byte2 >= 161 && byte2 <= 254)))     //判断是否是Big5编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        } 
        #endregion

        #region [IsOnlyContainsChinese]
        /// <summary>
        /// 给定一个字符串，判断其是否只包含有汉字
        /// </summary>
        /// <param name="testStr"></param>
        /// <returns></returns>
        public static bool IsOnlyContainsChinese(this string testStr)
        {
            char[] words = testStr.ToCharArray();
            foreach (char word in words)
            {
                if (IsGBCode(word.ToString()) || IsGBKCode(word.ToString()))  // it is a GB2312 or GBK chinese word
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }  
        #endregion
    }
}
