using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain
{
    public class PropertyMappingValue
    {
        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            DestinationProperties = destinationProperties;
            Revert = revert;
        }

        public IEnumerable<string> DestinationProperties { get; private set; }

        public bool Revert { get; private set; }
    }
}
