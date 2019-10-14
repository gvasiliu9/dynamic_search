using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Utmdev.DynamicSearch.Attributes;

namespace Utmdev.DynamicSearch
{
    public class SearchQuery<Model, SearchModel>
    {
        // Expression Tree parts
        ParameterExpression parameter = Expression.Parameter(typeof(Model), "item");
        Expression left;
        Expression right;
        MethodInfo method;
        PropertyInfo property;
        MemberExpression propertyAccess;
        LambdaExpression lambdaExpression;
        Expression expression;
        Expression predicate;

        // Items sources
        public IQueryable<Model> Source { get; set; }

        /// <summary>
        /// Run search query
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns></returns>
        public IQueryable<Model> Run(SearchModel searchViewModel)
        {
            // Check
            if (searchViewModel == null)
                throw new ArgumentNullException(nameof(SearchModel));

            // Get viewmodel type
            Type searchViewModelType = searchViewModel.GetType();

            // Get properties
            var properties = searchViewModelType.GetProperties()
                .Where(p => !p.IsDefined(typeof(ExcludeAttribute), false));

            if (!properties.Any())
                return null;

            // Build query
            try
            {
                foreach (var property in properties)
                {

                    // Check property
                    if (property == null || property.Name == "OrderBy")
                        continue;

                    // Get default value
                    var defaultValueAttribute =
                        (DefaultValueAttribute)Attribute
                            .GetCustomAttribute(property, typeof(DefaultValueAttribute));

                    // Get compare type
                    var compareAttribute =
                        (CompareAttribute)Attribute
                            .GetCustomAttribute(property, typeof(CompareAttribute));

                    var compareType = compareAttribute?.CompareType ?? Enums.CompareType.IsEqual;

                    // Convert values to string
                    var stringDefaultValue = defaultValueAttribute?.DefaultValue?.ToString();
                    var stringPropertyValue = property.GetValue(searchViewModel)?.ToString();

                    // Get property value and type
                    var propertyValue = property.GetValue(searchViewModel);
                    var propertyValueType = property.PropertyType;
                    var propertyName = property.Name;

                    // Append query
                    if (stringPropertyValue != stringDefaultValue)
                    {
                        left = Expression.Property(parameter, typeof(Model).GetProperty(propertyName));
                        right = Expression.Constant(propertyValue, propertyValueType);

                        // Is equal case
                        if (compareType == Enums.CompareType.IsEqual)
                        {
                            expression = Expression.Equal(left, right);
                        }
                        // Contains case
                        else if (compareType == Enums.CompareType.Contains)
                        {
                            left = Expression.Property(parameter, typeof(Model).GetProperty(propertyName));
                            method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                            right = Expression.Constant(propertyValue, typeof(string));

                            expression = Expression.Call(left, method, right);
                        }
                        // Is greater
                        else if (compareType == Enums.CompareType.IsGreater)
                        {
                            expression = Expression.GreaterThan(left, right);
                        }
                        // Is less
                        else if (compareType == Enums.CompareType.IsLess)
                        {
                            expression = Expression.LessThan(left, right);
                        }
                        // Is greater or equal
                        else if (compareType == Enums.CompareType.IsGreaterOrEqual)
                        {
                            expression = Expression.GreaterThanOrEqual(left, right);
                        }
                        // Is less or equal
                        else if (compareType == Enums.CompareType.IsLessOrEqual)
                        {
                            expression = Expression.LessThanOrEqual(left, right);
                        }

                        // Append to predicate
                        predicate = (predicate != null)
                            ? Expression.AndAlso(predicate, expression)
                            : expression;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Build predicate error");
            }

            // Check items source
            if (Source == null)
                throw new Exception("Source is not specified");

            // Execute search
            if (predicate != null)
            {
                try
                {
                    MethodCallExpression whereCallExpression = Expression.Call(typeof(Queryable),
                    "Where",
                    new Type[] { Source.ElementType },
                    Source.Expression,
                    Expression.Lambda<Func<Model, bool>>
                        (predicate, new ParameterExpression[] { parameter }));

                    Source = Source.Provider.CreateQuery<Model>(whereCallExpression);
                }
                catch
                {
                    throw new Exception("Where query error");
                }
            }

            // Execute order by
            try
            {
                var orderByProperty = searchViewModelType.GetProperties()
                .FirstOrDefault(p => p.Name == "OrderBy");

                var orderByPropertyValue = orderByProperty?.GetValue(searchViewModel);

                var orderByPropertyDefaultValueAttribute =
                    (DefaultValueAttribute)Attribute
                        .GetCustomAttribute(orderByProperty, typeof(DefaultValueAttribute));

                var stringOrderByPropertyValue = orderByPropertyValue?.ToString();
                var defaultOrderByPropertyValue = orderByPropertyDefaultValueAttribute?.DefaultValue?.ToString();

                if (defaultOrderByPropertyValue != stringOrderByPropertyValue && stringOrderByPropertyValue != null)
                {

                    property = typeof(Model).GetProperty(stringOrderByPropertyValue);
                    propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    lambdaExpression = Expression.Lambda(propertyAccess, parameter);

                    MethodCallExpression orderByCallExpression
                        = Expression.Call
                        (typeof(Queryable),
                        "OrderByDescending",
                        new Type[] { typeof(Model), property.PropertyType },
                        Source.Expression,
                        Expression.Quote(lambdaExpression));

                    Source = Source.Provider.CreateQuery<Model>(orderByCallExpression);
                }
            }
            catch
            {
                throw new Exception("OrderBy query error");
            }

            // Result
            return Source;
        }
    }
}
