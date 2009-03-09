
namespace System.Web.Mvc.Html
{
    public class NumberField : TextField, IRegField
    {
        public NumberField(string label, string name)
            : this(label, name, false)
        {
        }

        public NumberField(string label, string name, NumberMode mode)
            : this(label, name, false, mode)
        {
        }

        public NumberField(string label, string name, bool required)
            : this(label, name, required, NumberMode.Double)
        {
        }

        public NumberField(string label, string name, bool required, NumberMode mode)
            : base(label, name, required)
        {
            Mode = mode;
        }

        public new NumberMode Mode { get; set; }

        public override FieldType FieldType
        {
            get { return FieldType.Number; }
        }

        public override string GetReg()
        {
            string mode = this.Mode.ToString();

            string type = mode.Contains("Int") ? "int" : "double";

            if (mode.StartsWith("U"))
            {
                type += "+";
            }
            else if (mode.StartsWith("L"))
            {
                type += "-";
            }

            if (mode.Contains("Zero"))
            {
                type += "0";
            }

            return type;
        }
    }
}
