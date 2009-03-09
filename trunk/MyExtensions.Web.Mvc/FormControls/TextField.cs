
namespace System.Web.Mvc.Html
{
    public class TextField : TextFieldBase
    {
        public TextField(string label, string name)
            : this(label, name, false)
        {
        }

        public TextField(string label, string name,TextMode mode)
            : this(label, name, false, mode)
        {
        }

        public TextField(string label, string name, bool required)
            : this(label, name, required, TextMode.Any)
        {
        }

        public TextField(string label, string name, bool required, TextMode mode)
            : base(label, name, required, mode)
        {
        }

        public override FieldType FieldType
        {
            get { return FieldType.Text; }
        }
    }
}
