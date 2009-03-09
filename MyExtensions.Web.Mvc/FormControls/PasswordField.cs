using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc.Html
{
    public class PasswordField : TextFieldBase
    {
        public PasswordField(string label, string name)
            : this(label, name, false)
        {
        }

        public PasswordField(string label, string name, bool required)
            : base(label, name, required)
        {
        }

        public override FieldType FieldType
        {
            get { return FieldType.Password; }
        }
    }
}
