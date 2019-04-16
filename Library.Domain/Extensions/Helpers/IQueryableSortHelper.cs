namespace Library.Domain.Extensions.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;

    internal static class IQueryableSortHelper
    {
        private const string DescendingKeyword = " desc";
        private const string PropertyMappingValueMissing = "propertyMappingValue is missing";
        private const string DescendingOrder = " descending";
        private const string AscendingOrder = " ascending";
        private const string PropertyAndSortOrderDelimiter = " ";
        private const char GroupDelimiter = ',';

        internal static IQueryable<T> Sort<T>(IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            var orderByAfterSplit = orderBy.Split(GroupDelimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                source = ApplyOrderByClause(source, mappingDictionary, orderByClause);
            }

            return source;
        }

        internal static bool Validate(string fields, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            var result = true;

            var orderByAfterSplit = fields.Split(GroupDelimiter, StringSplitOptions.RemoveEmptyEntries);

            foreach (var orderByClause in orderByAfterSplit)
            {
                result = ValidMappingExistsForField(mappingDictionary, orderByClause.Trim());

                if (result == false)
                {
                    break;
                }
            }

            return result;
        }

        private static bool ValidMappingExistsForField(Dictionary<string, PropertyMappingValue> mappingDictionary, string orderByClause)
        {
            var trimmedOrderByClause = orderByClause.Trim();

            string propertyName = GetPropertyName(trimmedOrderByClause);

            return mappingDictionary.ContainsKey(propertyName);
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
    }
}
