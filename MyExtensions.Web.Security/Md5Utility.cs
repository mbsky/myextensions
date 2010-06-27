using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Web;

namespace MyExtensions.Web.Security
{
    public static class Md5Utility
    {
        public static string md5(string str, Md5Length code)
        {
            if (code == Md5Length.Sixteen)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 0x10);
            }
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();

            //if (code == Md5Length.Sixteen)
            //{
            //    return MD5(str, "gb2312").ToLower().Substring(8, 0x10);
            //}

            //return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }


        /**/
        /// <summary> 
        /// 对字符串进行MD5加密 
        /// </summary> 
        /// <param name="text">要加密的字符串</param> 
        /// <param name="charset">字符串编码格式</param> 
        /// <example>str = MD5("木子屋","gb2312");</example> 
        /// <returns></returns> 
        public static string MD5(string text, string charset)
        {
            return (MD5(text, charset, false));
        }

        /**/
        /// <summary> 
        /// 对字符串或参数值进行MD5加密 
        /// </summary> 
        /// <param name="text">要加密的字符串或参数名称</param> 
        /// <param name="charset">字符串编码格式</param> 
        /// <param name="isArg">加密字符串类型　true:参数值 false:字符串</param> 
        /// <returns></returns> 
        public static string MD5(string text, string charset, bool isArg)
        {
            try
            {
                MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

                HttpRequest request = HttpContext.Current.Request;

                if (isArg)
                {
                    NameValueCollection Collect = HttpUtility.ParseQueryString(request.Url.Query, Encoding.GetEncoding(charset));//使用Collect接收参数值 
                    if (Collect[text] != null)
                    {
                        return BitConverter.ToString(MD5.ComputeHash(Encoding.GetEncoding(charset).GetBytes(Collect[text].ToString()))).Replace("-", "");
                    }
                }
                else
                {
                    return BitConverter.ToString(MD5.ComputeHash(Encoding.GetEncoding(charset).GetBytes(text))).Replace("-", "");
                }
            }
            catch { }

            return string.Empty;
        }

    }
}
