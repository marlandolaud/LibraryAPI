namespace Library.Domain.Services
{
    using Library.Contracts.Response.Author;
    using Library.Domain.Entities;
    using Library.Domain.Extensions.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _authorPropertyMapping;

        private readonly IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _authorPropertyMapping = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>(){ nameof(Author.Id)}) },
                {"Genre", new PropertyMappingValue(new List<string>(){ nameof(Author.Genre) }) },
                {"Age", new PropertyMappingValue(new List<string>(){ nameof(Author.DateOfBirth) }) },
                {"Name", new PropertyMappingValue(new List<string>(){ nameof(Author.FirstName), nameof(Author.LastName) })},
            };
            propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var match = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            try
            {
                return match.Single().MappingDictionary;
            }
            catch (InvalidOperationException)
            {
                //log error

                throw;
            }
        }

        public bool ValidMappingExistsForFields<TSource, TDestination>(string fields)
        {
            bool result = false;

            var match = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return result;
            }

            result = IQueryableSortHelper.Validate(fields, match.Single().MappingDictionary);

            return result;
        }
    }
}
