using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.UI.WebControls;

namespace System.Web.Mvc.Html
{
    public class SelectFieldList : ListField
    {
        public SelectFieldList(string label, string name)
            : this(label, name, false)
        {
        }

        public SelectFieldList(string label, string name, bool required)
            : base(label, name, required)
        {
        }

        public SelectFieldList(string label, string name, params ListItem[] options)
            : this(label, name, false, options)
        {
        }

        public SelectFieldList(string label, string name, IEnumerable<ListItem> options)
            : this(label, name, false, options)
        {
        }

        public SelectFieldList(string label, string name, IEnumerable options, string dataValueField, string dataTextField)
            : this(label, name, false, options, dataValueField, dataTextField)
        {
        }

        public SelectFieldList(string label, string name, bool required, IEnumerable<ListItem> options)
            : this(label, name, required, options, "Value", "Text")
        {
        }

        public SelectFieldList(string label, string name, bool required, IEnumerable options, string dataValueField, string dataTextField)
            : base(label, name, required, new SelectList(options, dataValueField, dataTextField))
        {
        }

        public SelectFieldList(string label, string name, Type enumType)
            : this(label, name, false, enumType.ToListItems())
        {
        }

        public SelectFieldList(string label, string name, bool required, Type enumType)
            : this(label, name, required, enumType.ToListItems())
        {
        }

        public override FieldType FieldType
        {
            get { return FieldType.Select; }
        }
    }
}
