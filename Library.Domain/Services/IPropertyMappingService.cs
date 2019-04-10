﻿namespace Library.Domain.Services
{
    using System.Collections.Generic;

    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}
