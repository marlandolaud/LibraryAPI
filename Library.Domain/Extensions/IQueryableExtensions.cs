namespace Library.Domain.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;

    public static class IQueryableExtensions
    {
        private const string DescendingKeyword = " desc";
        private const string PropertyMappingValueMissing = "propertyMappingValue is missing";
        private const string MappingDictionaryMissing = "mappingDictionary is missing";
        private const string SourceMissing = "source is missing";
        private const string DescendingOrder = " descending";
        private const string AscendingOrder = " ascending";
        private const string PropertyAndSortOrderDelimiter = " ";
        private const char GroupDelimiter = ',';

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException(SourceMissing);
            }

            if (mappingDictionary == null)
            {
                throw new ArgumentNullException(MappingDictionaryMissing);
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(GroupDelimiter);

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                source = ApplyOrderByClause(source, mappingDictionary, orderByClause);
            }

            return source;
        }

        /// <summary>
        /// apply each OrderBy clause in reverse order - otherwise, the IQueryable will be ordered in the wrong order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="mappingDictionary"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        private static IQueryable<T> ApplyOrderByClause<T>(IQueryable<T> source, Dictionary<string, PropertyMappingValue> mappingDictionary, string orderByClause)
        {
            var trimmedOrderByClause = orderByClause.Trim();

            var isDescending = trimmedOrderByClause.EndsWith(DescendingKeyword);

            string propertyName = GetPropertyName(trimmedOrderByClause);

            if (!mappingDictionary.ContainsKey(propertyName))
            {
                throw new ArgumentException($"Key mapping for {propertyName} is missing");
            }

            // get the PropertyMappingValue
            var propertyMappingValue = mappingDictionary[propertyName];

            if (propertyMappingValue == null)
            {
                throw new ArgumentNullException(PropertyMappingValueMissing);
            }

            // Run through the property names in reverse so the OrderBy clauses are applied in the correct order
            foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
            {
                // revert sort order if necessary
                if (propertyMappingValue.Revert)
                {
                    isDescending = !isDescending;
                }

                source = source.OrderBy(destinationProperty + (isDescending ? DescendingOrder : AscendingOrder));
            }

            return source;
        }

        private static string GetPropertyName(string trimmedOrderByClause)
        {
            var indexOfFirstSpace = trimmedOrderByClause.IndexOf(PropertyAndSortOrderDelimiter);

            var propertyName = indexOfFirstSpace == -1 ?
                trimmedOrderByClause :
                trimmedOrderByClause.Remove(indexOfFirstSpace);
            return propertyName;
        }

        //public static IQueryable<object> ShapeData<TSource>(this IQueryable<TSource> source,
        //    string fields,
        //   Dictionary<string, PropertyMappingValue> mappingDictionary)
        //{
        //    if (source == null)
        //    {
        //        throw new ArgumentNullException("source");
        //    }

        //    if (mappingDictionary == null)
        //    {
        //        throw new ArgumentNullException("mappingDictionary");
        //    }

        //    if (string.IsNullOrWhiteSpace(fields))
        //    {
        //        return (IQueryable<object>)source;
        //    }

        //    // ignore casing
        //    fields = fields.ToLower();

        //    // the field are separated by ",", so we split it.
        //    var fieldsAfterSplit = fields.Split(',');

        //    // select clause starts with "new" - will create anonymous objects
        //    var selectClause = "new (";

        //    // run through the fields
        //    foreach (var field in fieldsAfterSplit)
        //    {
        //        // trim each field, as it might contain leading 
        //        // or trailing spaces. Can't trim the var in foreach,
        //        // so use another var.
        //        var propertyName = field.Trim();

        //        // find the matching property
        //        if (!mappingDictionary.ContainsKey(propertyName))
        //        {
        //            throw new ArgumentException($"Key mapping for {propertyName} is missing");
        //        }

        //        // get the PropertyMappingValue
        //        var propertyMappingValue = mappingDictionary[propertyName];

        //        if (propertyMappingValue == null)
        //        {
        //            throw new ArgumentNullException("propertyMappingValue");
        //        }

        //        // Run through the destination property names
        //        foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
        //        {
        //            // add to select clause
        //            selectClause += $" {destinationProperty},";
        //        }
        //    }

        //    // remove last comma, add closing arrow and execute select clause
        //    selectClause = selectClause.Substring(0, selectClause.Length - 1) + ")";
        //    return (IQueryable<object>)source.Select(selectClause);
        //}
    }

}
