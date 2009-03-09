using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace System.Web.Mvc.Html
{
    public class RadioFieldList : ListField
    {
        public RadioFieldList(string label, string name)
            : this(label, name, false)
        {
        }

        public RadioFieldList(string label, string name, bool required)
            : base(label, name, required)
        {
        }

        public RadioFieldList(string label, string name, params ListItem[] options)
            : this(label, name, false, options)
        {
        }

        public RadioFieldList(string label, string name, IEnumerable<ListItem> options)
            : this(label, name, false, options)
        {
        }

        public RadioFieldList(string label, string name, IEnumerable options, string dataValueField, string dataTextField)
            : this(label, name, false, options, dataValueField, dataTextField)
        {
        }

        public RadioFieldList(string label, string name, bool required, IEnumerable<ListItem> options)
            : this(label, name, required, options, "Value", "Text")
        {
        }

        public RadioFieldList(string label, string name, bool required, IEnumerable options, string dataValueField, string dataTextField)
            : base(label, name, required, new SelectList(options, dataValueField, dataTextField))
        {
        }

        public RadioFieldList(string label, string name, Type enumType)
            : this(label, name, false, enumType.ToListItems())
        {
        }

        public RadioFieldList(string label, string name, bool required, Type enumType)
            : this(label, name, required, enumType.ToListItems())
        {
        }

        public override FieldType FieldType
        {
            get { return FieldType.RadioList; }
        }
    }
}
