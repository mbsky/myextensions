using System.Linq.Expressions;

namespace System.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicPropertyAccessor
    {
        private Func<object, object> m_getter;

        public DynamicPropertyAccessor(Type type, string propertyName)
            : this(type.GetProperty(propertyName))
        { }

        public DynamicPropertyAccessor(PropertyInfo propertyInfo)
        {
            // target: (object)((({TargetType})instance).{Property})

            // preparing parameter, object type
            ParameterExpression instance = Expression.Parameter(
                typeof(object), "instance");

            // ({TargetType})instance
            Expression instanceCast = Expression.Convert(
                instance, propertyInfo.ReflectedType);

            // (({TargetType})instance).{Property}
            Expression propertyAccess = Expression.Property(
                instanceCast, propertyInfo);

            // (object)((({TargetType})instance).{Property})
            UnaryExpression castPropertyValue = Expression.Convert(
                propertyAccess, typeof(object));

            // Lambda expression
            Expression<Func<object, object>> lambda =
                Expression.Lambda<Func<object, object>>(
                    castPropertyValue, instance);

            this.m_getter = lambda.Compile();
        }

        public object GetValue(object o)
        {
            return this.m_getter(o);
        }

    }
}
