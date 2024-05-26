using System.Linq.Expressions;
using System.Reflection;

namespace TournamentSystemDataSource.Extensions
{
    public static class DynamicLinqFilter
    {
        /// <summary>
        /// Dynamicaly create filter and apply it to the db query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="propertyValueType">For example: "System.Int32"</param>
        /// <returns></returns>
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, PropertyInfo propertyInfo, object propertyValue, string propertyValueType)
        {
            var propertyName= propertyInfo.Name;
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);

            var propertyType = propertyInfo.PropertyType;

            Expression body;
            if (propertyType == typeof(string) && propertyValueType == "System.String")
            {
                body = Expression.Call(property, "Contains", null, Expression.Constant(propertyValue));
            }
            else
            {
                var constant = Expression.Constant(Convert.ChangeType(propertyValue, propertyType));
                body = Expression.Equal(property, constant);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

            return query.Where(lambda);
        }
    }
}
