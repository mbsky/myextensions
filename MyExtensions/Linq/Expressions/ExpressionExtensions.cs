
namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        #region TryGetEntityAndFieldNameFromExpression
        public static bool TryGetEntityAndFieldNameFromExpression(this Expression expression, out object entity, out string fieldName)
        {
            entity = null;
            fieldName = null;

            try
            {
                MemberExpression memberExpression = expression as MemberExpression;

                if (memberExpression != null)
                {
                    var entityLambda = Expression.Lambda<Func<object>>(memberExpression.Expression);
                    entity = entityLambda.Compile()();
                    fieldName = memberExpression.Member.Name;
                    return true;
                }

                UnaryExpression unaryExpression = expression as UnaryExpression;

                if (unaryExpression != null)
                    if (unaryExpression.NodeType == ExpressionType.Convert || unaryExpression.NodeType == ExpressionType.MemberAccess)
                        return TryGetEntityAndFieldNameFromExpression(unaryExpression.Operand, out entity, out fieldName);
            }
            catch (Exception) { }

            return false;
        } 
        #endregion

        #region PredicateExtensions http://www.talentgrouplabs.com/blog/archive/2007/11/26/dynamic-linq-queries--dynamic-where-clause-part-2.aspx

        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(Expression.Or(expression1.Body, invokedExpression), expression1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(Expression.And(expression1.Body, invokedExpression), expression1.Parameters);
        } 
        #endregion
    }
}
