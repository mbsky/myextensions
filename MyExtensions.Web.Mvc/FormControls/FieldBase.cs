using System.Collections.Generic;
using System.Globalization;
using System;

namespace System.Web.Mvc.Html
{

    public abstract class FieldBase : Dictionary<string, object>, IDictionary<string, object> {
        public FieldBase(string label, string name)
            : this(label, name, false) {
        }

        public FieldBase(string label, string name, bool required) {
            if (this.FieldType != FieldType.Hidden)
                Check.AssertNotNullOrEmpty(label, "label");

            Check.AssertNotNullOrEmpty(name, "name");

            Label = label;
            Name = name;
            Required = required;
        }

        /// <summary>
        /// 标签
        /// </summary>
        public virtual string Label { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 从这个字段开始新的分组的分组名称
        /// </summary>
        public virtual string GroupName { get; set; }

        /// <summary>
        /// OneRowFlag不为空 并且有相同的OneRowFlag值的Field 输出在同一行
        /// </summary>
        public virtual string OneRowFlag { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public virtual string Tip { get; set; }

        public virtual bool Required { get; set; }

        //public virtual bool ShowSeperatorOnEnd { get; set; }

        public abstract FieldType FieldType { get; }

        public virtual string VType { get; set; }

        public virtual FieldClientConfig GetClientConfig() {
            throw new NotImplementedException();
        }

        internal static string MapIntegerAttributeToString(int n) {
            if (n == -1) {
                return null;
            }
            return n.ToString(NumberFormatInfo.InvariantInfo);
        }
    }
}
