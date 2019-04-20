
namespace Library.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class TypeHelperService : ITypeHelperService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(",", StringSplitOptions.RemoveEmptyEntries);

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(T)
                    .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
