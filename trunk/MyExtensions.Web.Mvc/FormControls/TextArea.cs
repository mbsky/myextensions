using System;
using System.Globalization;

namespace System.Web.Mvc.Html
{
    public class TextArea : TextFieldBase
    {

        public TextArea(string label, string name)
            : this(label, name, false)
        {
        }

        public TextArea(string label, string name, int? rows)
            : this(label, name, false, rows)
        {
        }

        public TextArea(string label, string name, bool required)
            : this(label, name, required, null)
        {
        }

        public TextArea(string label, string name, bool required, int? rows)
            : base(label, name, required)
        {
            Rows = rows;
        }

        public int? Rows
        {
            get
            {
                if (this.ContainsKey("rows"))
                    return Int32.Parse(this["rows"].ToString(), CultureInfo.InvariantCulture);

                return null;
            }
            set
            {
                if (null == value || value <= 0)
                    this.Remove("rows");
                else

                    this["rows"] = FieldBase.MapIntegerAttributeToString(value.Value);
            }
        }

        public override FieldType FieldType
        {
            get { return FieldType.TextArea; }
        }
    }
}
