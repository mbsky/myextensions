using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc.Html
{
    public class Hidden : FieldBase {

        public Hidden(string name)
            : base(null, name, false) {
        }

        public virtual string Value { get; set; }

        public Hidden(string name,string value)
            : base(null, name, false)
        {
            Value = value;
        }

        public override FieldType FieldType {
            get { return FieldType.Hidden; }
        }
    }
}
