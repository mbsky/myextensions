using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Web.Mvc.Controls;
using System.Web.UI.WebControls;

namespace System.Web.Mvc.Html
{
    public class CheckboxList :ListField
    {
        public CheckboxList(string label, string name, params ListItem[] options)
            : this(label, name, false, options)
        {
        }

        public CheckboxList(string label, string name, bool required, params ListItem[] options)
            : this(label, name, required, (IEnumerable<ListItem>)options)
        {
        } 

        public CheckboxList(string label, string name, IEnumerable<ListItem> options)
            : this(label, name, false, options)
        {
        }

        public CheckboxList(string label, string name, bool required, IEnumerable<ListItem> options)
            : base(label, name, required, new SelectList(options, "Value", "Text"))
        {
        }

        public override FieldType FieldType
        {
            get { return FieldType.CheckBoxList; }
        }
    }
}
