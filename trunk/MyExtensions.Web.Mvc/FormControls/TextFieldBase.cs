using System;
using System.Globalization;

namespace System.Web.Mvc.Html
{
    public enum TextMode
    {
        Any,
        Custom,
        Letter,
        UpperChar,
        LowerChar,
        DigitChar,
        DigitChar_,
        /// <summary>
        /// 中文
        /// </summary>
        Chinese,
        Zipcode,
        Color,
        /// <summary>
        /// 
        /// </summary>
        Tel,
        /// <summary>
        /// 用来用户注册。匹配由数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        Username,
        Idcard,
        Picture
    }

    public abstract class TextFieldBase : FieldBase, IRegField  
    {
        public TextFieldBase(string label, string name, bool required)
            : base(label, name, required)
        {
        }

        public TextFieldBase(string label, string name, bool required,TextMode mode)
            : base(label, name, required)
        {
            Mode = mode;
        }

        /// <summary>
        /// Gets or sets the width of the text box.
        /// </summary>
        public virtual int? Size
        {
            get
            {
                if (this.ContainsKey("size"))
                    return Int32.Parse(this["size"].ToString(), CultureInfo.InvariantCulture);

                return null;
            }
            set
            {
                if (null == value || value <= 0)
                    this.Remove("size");
                else

                    this["size"] = FieldBase.MapIntegerAttributeToString(value.Value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters that can be entered in the text box.
        /// </summary>
        public virtual int? MaxLength
        {
            get
            {
                if (this.ContainsKey("maxLength"))
                    return Int32.Parse(this["maxLength"].ToString(), CultureInfo.InvariantCulture);

                return null;
            }
            set
            {
                if (null == value || value <= 0)
                {
                    this.Remove("maxLength");
                }
                else

                    this["maxLength"] = FieldBase.MapIntegerAttributeToString(value.Value);
            }
        }

        public virtual TextMode Mode { get; set; }

        //public virtual string CustomPattern { get; set; }

        public virtual string GetReg()
        {
            if (this.Mode == TextMode.Any)
                return string.Empty;

            if (this.Mode == TextMode.Custom && VType.IsNullOrEmpty() == false)
                return VType;

            return this.Mode.ToString().ToLower();
        }
    }
}
