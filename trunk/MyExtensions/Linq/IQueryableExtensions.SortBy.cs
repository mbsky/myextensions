using System.Linq.Expressions;
using System.Reflection;
using System.Security.Permissions;
using System.Web;

namespace System.Linq
{
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class IQueryableSortExtensions
    {
        #region FirstOrDefault
        public static object FirstOrDefault(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            foreach (object obj in source)
                return obj;

            return null;
        } 
        #endregion

        #region SortBy
        /// <summary>
        /// SortBy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            // DataSource control passes the sort parameter with a direction 
            // if the direction is descending           
            int descIndex = propertyName.IndexOf("DESC", StringComparison.OrdinalIgnoreCase);
            if (descIndex >= 0)
            {
                propertyName = propertyName.Substring(0, descIndex).Trim();
            }

            if (String.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = (descIndex < 0) ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));

            return (IQueryable<T>)((IQueryable)source).Provider.CreateQuery(methodCallExpression);
        }
        #endregion

        #region ApplyOrderByClause
        public static IQueryable<T> ApplyOrderByClause<T>(this IQueryable<T> query, string column)
        {
            PropertyInfo columnPropInfo = typeof(T).GetProperty(column);

            var entityParam = Expression.Parameter(typeof(T), "e");                    // {e}
            var columnExpr = Expression.MakeMemberAccess(entityParam, columnPropInfo); // {e.column}
            var lambda = Expression.Lambda(columnExpr, entityParam);                   // {e => e.column}

            var call = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { typeof(T), columnPropInfo.PropertyType }, query.Expression, lambda);

            query = query.Provider.CreateQuery<T>(call);

            return query;
        }

        public static IQueryable<T> ApplyOrderByDescendingClause<T>(this IQueryable<T> query, string column)
        {
            PropertyInfo columnPropInfo = typeof(T).GetProperty(column);

            var entityParam = Expression.Parameter(typeof(T), "e");                    // {e}
            var columnExpr = Expression.MakeMemberAccess(entityParam, columnPropInfo); // {e.column}
            var lambda = Expression.Lambda(columnExpr, entityParam);                   // {e => e.column}

            var call = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { typeof(T), columnPropInfo.PropertyType }, query.Expression, lambda);

            query = query.Provider.CreateQuery<T>(call);

            return query;
        }
        #endregion
    }
}
