using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensionsMatch
    {
        public static string GetSplit(this string _source, string regex, int index)
        {
            if (_source.IsNullOrEmpty())
                return string.Empty;

            var matches = Regex.Split(_source, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return matches[index];
        }

        public static string GetMatch(this string _source, string regex, int groupIndex)
        {
            if (_source.IsNullOrEmpty())
                return string.Empty;

            Match TitleMatch = Regex.Match(_source, regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (TitleMatch.Success)

                return TitleMatch.Groups[groupIndex].Value;

            return string.Empty;
        }

        public static string GetFirstMatch(this string _source, string regex)
        {
            return GetMatch(_source, regex, 1);
        }

        public static string GetValueUseRegex(this string text, string regex)
        {
            return GetValueUseRegex(text, regex, "data");
        }

        public static string GetValueUseRegex(this string text, string regex, string groupName)
        {
            Regex rgClass = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            Match match = rgClass.Match(text);

            if (match.Success)
                return match.Groups[groupName].Value;

            return string.Empty;
        }

        public static string GetFirstMatch(this string _source, string start, string end)
        {
            return _source.GetFirstMatch(start, end, true, true);
        }

        public static string GetFirstMatch(this string _source, string start, string end, bool appendStart, bool appendEnd)
        {
            string groupName = "content";
            string regex = @"^.*" + start + "(?<" + groupName + ">.+?)" + end + ".*$";

            return (appendStart ? start : string.Empty)
                + _source.GetValueUseRegex(regex, groupName)
                + (appendEnd ? end : string.Empty);
        }

        #region WipeScript
        static Regex regex1 = new Regex(@"<script[\s\s]+</script *>", RegexOptions.IgnoreCase);
        static Regex regex2 = new Regex(@" href *= *[\s\s]*script *:", RegexOptions.IgnoreCase);
        static Regex regex3 = new Regex(@" on[\s\s]*=", RegexOptions.IgnoreCase);
        static Regex regex4 = new Regex(@"<iframe[\s\s]+</iframe *>", RegexOptions.IgnoreCase);
        static Regex regex5 = new Regex(@"<frameset[\s\s]+</frameset *>", RegexOptions.IgnoreCase);

        /// <summary>
        /// 清楚脚本、iframe、on事件
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string WipeScript(this string html)
        {
            html = regex1.Replace(html, String.Empty); //过滤<script></script>标记   
            html = regex2.Replace(html, String.Empty); //过滤href=javascript: (<a>) 属性   
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件   
            html = regex4.Replace(html, String.Empty); //过滤iframe   
            html = regex5.Replace(html, String.Empty); //过滤frameset   
            return html;
        }
        #endregion

    }
}
