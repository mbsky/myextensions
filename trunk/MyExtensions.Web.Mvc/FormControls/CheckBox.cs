
namespace System.Web.Mvc.Html
{
    public class Checkbox : FieldBase
    {

        public Checkbox(string label, string name)
            : this(label, name, false)
        {
        }

        public Checkbox(string label, string name, bool required)
            : base(label, name, required)
        {
        }

        public override FieldType FieldType
        {
            get { return FieldType.CheckBox; }
        }
    }
}
