
namespace System.Web.Mvc.Html
{
    public class EmailField : TextField, IRegField
    {
        public EmailField(string name)
            : this(defaultLabel, name, false)
        {
        }

        public EmailField(string name, bool required)
            : this(defaultLabel, name, required)
        {
        }

        public EmailField(string label, string name)
            : this(label, name, false)
        {
        }

        public EmailField(string label, string name, bool required)
            : base(label, name, required)
        {
        }

        static readonly string vtype = "email";
        static readonly string defaultLabel = "E-Mail";

        //public override string VType
        //{
        //    get
        //    {
        //        return vtype;
        //    }
        //}

        public override string GetReg()
        {
            return vtype;
        }
    }
}
