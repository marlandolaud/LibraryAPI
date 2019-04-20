namespace Library.Domain.Extensions
{
    using Library.Domain.Extensions.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Reflection;
    using System.Text;

    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentException("source");
            }

            var result = new List<ExpandoObject>();

            // create a list with propertyInfo object on TSource. Reflection is expensive, so rather than doing it for each obj in the list, we do it once and reuse the results
            // after all part of the reflection is on the type of the object (TSource), not the instance
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyList = typeof(TSource)
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyList);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(",", StringSplitOptions.RemoveEmptyEntries);

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} was not found on {typeof(TSource)}");
                    }

                    propertyInfoList.Add(propertyInfo);
                }
            }

            // run through the source objects
            foreach (TSource sourceObject in source)
            {
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }

                result.Add(dataShapedObject);
            }

            return result;
        }
    }
}
