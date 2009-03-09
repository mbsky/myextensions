using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc.Html
{
    public class ImageField : FieldBase
    {
        public ImageField(string label, string name, bool required)
            : base(label, name, required)
        {
        }

        public override FieldType FieldType
        {
            get { return FieldType.Image; }
        }
    }
}
