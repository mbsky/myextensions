using System.ComponentModel;

namespace System.Reflection
{
    public static class PropertyInfoExtensions
    {
        public static object GetDefaultValue(this PropertyInfo property)
        {
            DefaultValueAttribute att = property.GetAttribute<DefaultValueAttribute>(false);
            if (null != att)
            {
                return att.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Get public field/property value
        /// </summary>
        public static object GetFieldValue(object instance, string fieldName)
        {
            Check.Require(instance, "instance");
            Check.Require(fieldName, "fieldName", Check.NotNullOrEmpty);

            Type t = instance.GetType();

            System.Reflection.PropertyInfo pi = t.DeepGetProperty(fieldName);
            if (pi != null)
            {
                return pi.GetValue(instance, null);
            }

            System.Reflection.FieldInfo fi = t.DeepGetField(fieldName, true, false, false);
            if (fi != null)
            {
                return fi.GetValue(instance);
            }

            return null;
        }

        /// <summary>
        /// Set public field/property value
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        public static void SetFieldValue(this object instance, string fieldName, object fieldValue)
        {
            Check.Require(instance, "instance");
            Check.Require(fieldName, "fieldName", Check.NotNullOrEmpty);

            Type t = instance.GetType();

            PropertyInfo pi = t.DeepGetProperty(fieldName);
            if (pi != null)
            {
                pi.SetValue(instance, fieldValue, null);
            }

            FieldInfo fi = t.DeepGetField(fieldName, true, false, false);
            if (fi != null)
            {
                fi.SetValue(instance, fieldValue);
            }
        }
    }
}
