using System.Collections.Generic;

namespace Library.Domain
{
    public class PropertyMapping<T1, T2> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> authorPropertyMapping)
        {
            MappingDictionary = authorPropertyMapping;
        }
    }
}