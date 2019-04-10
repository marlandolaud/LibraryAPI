namespace Library.Domain.Infrastructure
{
    using Library.Domain.Repositories;
    using Library.Domain.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class DomainDependencyInjectionBinder
    {
        public static void Bind(IServiceCollection services)
        {
            services.AddSingleton<ILibraryRepository, LibraryRepository>();
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
        }
    }
}