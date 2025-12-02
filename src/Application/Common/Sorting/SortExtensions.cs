using System.Linq.Expressions;
using System.Reflection;

namespace BackendAuthTemplate.Application.Common.Sorting
{
    public static class SortExtensions
    {
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, ISortable sortable, ISortConfigurator<T> configurator)
        {
            if (sortable.Sorts == null || sortable.Sorts.Count == 0)
            {
                return query;
            }

            SortableProperties<T> sortableProperties = configurator.Configure();
            List<Sort> validSorts = sortableProperties.GetValidSorts(sortable.Sorts);

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

            for (int i = 0; i < validSorts.Count; i++)
            {
                Sort sort = validSorts[i];

                Expression? propertyAccess = CreateNestedPropertyAccess(parameter, sort.PropertyName);
                if (propertyAccess == null)
                {
                    continue;
                }

                LambdaExpression orderByExp = Expression.Lambda(propertyAccess, parameter);
                Type propertyType = ((PropertyInfo)((MemberExpression)propertyAccess).Member).PropertyType;

                string methodName;
                if (i == 0)
                {
                    methodName = sort.Direction == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";
                }
                else
                {
                    methodName = sort.Direction == SortDirection.Ascending ? "ThenBy" : "ThenByDescending";
                }

                query = (IQueryable<T>)typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), propertyType)
                    .Invoke(null, [query, orderByExp])!;
            }

            return query;
        }

        private static Expression? CreateNestedPropertyAccess(Expression parameter, string propertyName)
        {
            Expression propertyAccess = parameter;
            foreach (string property in propertyName.Split('.'))
            {
                PropertyInfo? propertyInfo = GetProperty(propertyAccess.Type, property);
                if (propertyInfo == null)
                {
                    return null;
                }

                propertyAccess = Expression.Property(propertyAccess, propertyInfo);
            }
            return propertyAccess;
        }

        private static PropertyInfo? GetProperty(Type type, string propertyName)
        {
            return type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
