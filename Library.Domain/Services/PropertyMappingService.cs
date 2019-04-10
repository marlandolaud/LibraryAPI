﻿namespace Library.Domain.Services
{
    using Library.Contracts.Response.Author;
    using Library.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>(){"Id"}) },
                {"Genre", new PropertyMappingValue(new List<string>(){"Genre"}) },
                {"Age", new PropertyMappingValue(new List<string>(){"DateOfBirth"}) },
                {"Name", new PropertyMappingValue(new List<string>(){"FirstName", "LastName"}) },
            };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
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
    }
}