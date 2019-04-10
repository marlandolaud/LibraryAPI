using System.Collections.Generic;

namespace Library.Domain.Services
{
    public class PropertyMapping<T1, T2> : IPropertyMapping
    {
        private Dictionary<string, PropertyMappingValue> _authorPropertyMapping;

        public PropertyMapping(Dictionary<string, PropertyMappingValue> authorPropertyMapping)
        {
            _authorPropertyMapping = authorPropertyMapping;
        }

        public Dictionary<string, PropertyMappingValue> MappingDictionary { get => _authorPropertyMapping; }
    }
}