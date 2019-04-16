namespace Library.Domain.Extensions
{
    using Library.Domain.Extensions.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;

    public static class IQueryableExtensions
    {
        private const string MappingDictionaryMissing = "mappingDictionary is missing";
        private const string SourceMissing = "source is missing";

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

            source = IQueryableSortHelper.Sort(source, orderBy, mappingDictionary);

            return source;
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
