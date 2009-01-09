using System.Linq.Expressions;

namespace System.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicPropertyAccessor
    {
        public PropertyInfo Property
        {
            get;
            private set;
        }

        private Func<object, object> m_getter;

        private DynamicExecutor m_dynamicSetter;

        public DynamicPropertyAccessor(Type type, string propertyName)
            : this(type.GetProperty(propertyName))
        { }

        public DynamicPropertyAccessor(PropertyInfo propertyInfo)
        {
            Property = propertyInfo;
        }

        private void prepareForGet()
        {
            // target: (object)((({TargetType})instance).{Property})

            // preparing parameter, object type
            ParameterExpression instance = Expression.Parameter(
                typeof(object), "instance");

            // ({TargetType})instance
            Expression instanceCast = Expression.Convert(
                instance, Property.ReflectedType);

            // (({TargetType})instance).{Property}
            Expression propertyAccess = Expression.Property(
                instanceCast, Property);

            // (object)((({TargetType})instance).{Property})
            UnaryExpression castPropertyValue = Expression.Convert(
                propertyAccess, typeof(object));

            // Lambda expression
            Expression<Func<object, object>> lambda =
                Expression.Lambda<Func<object, object>>(
                    castPropertyValue, instance);

            this.m_getter = lambda.Compile();

        }

        private void prepareForSet()
        {
            MethodInfo setMethod = Property.GetSetMethod();

            if (setMethod != null)
            {
                this.m_dynamicSetter = new DynamicExecutor(setMethod);
            }
            else
            {
                throw new NotSupportedException("Cannot set the property.");
            }
        }

        public object GetValue(object o)
        {
            if (null == this.m_getter)
            {
                prepareForGet();
            }

            return this.m_getter(o);
        }

        public void SetValue(object o, object value)
        {
            if (this.m_dynamicSetter == null)
            {
                prepareForSet();
            }

            this.m_dynamicSetter.Execute(o, new object[] { value });
        }
    }
}
