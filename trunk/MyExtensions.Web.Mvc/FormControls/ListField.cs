using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace System.Web.Mvc.Html
{
    public abstract class ListField : FieldBase
    {

        #region Hide Methods Without options param

        protected ListField(string label, string name)
            : base(label, name, false)
        {
        }

        protected ListField(string label, string name, bool required)
            : base(label, name, required)
        {
        }

        #endregion

        //public virtual Store Store { get; set; }

        #region fill with options

        //public ListField(string label, string name, Hashtable options)
        //    : this(label, name, false, options)
        //{
        //}

        //public ListField(string label, string name, NameValueCollection options)
        //    : this(label, name, false, options)
        //{
        //}

        //public ListField(string label, string name, string[] options)
        //    : this(label, name, false, options)
        //{
        //}

        //public ListField(string label, string name, params KeyValuePair<object, string>[] options)
        //    : this(label, name, false, options)
        //{
        //}

        //public ListField(string label, string name, IEnumerable options)
        //    : this(label, name, false, options)
        //{
        //}

        public ListField(string label, string name, bool required, SelectList options)
            : base(label, name, required)
        {
            Check.AssertNotNullOrEmpty(options, "options");

            Options = options;
        }
        #endregion


        public virtual SelectList Options { get; set; }
    }

    public static class ListFieldExtensions
    {
        public static IEnumerable<ListItem> ToListItems(this Type enumType)
        {
            if (!enumType.IsSubclassOf(typeof(Enum)))
            {
                throw new InvalidCastException();
            }

            FieldInfo[] staticFiles = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

            List<ListItem> items = new List<ListItem>(staticFiles.Length);

            foreach (FieldInfo fi in staticFiles)
            {


                items.Add(new ListItem()
                {
                    Text = fi.Name,
                    Value = fi.GetRawConstantValue().ToString() // fi.Name
                });
            }

            return items;
        }
    }
}
