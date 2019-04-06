namespace Library.Domain.Infrastructure
{
    using Library.Domain.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class DomainDependencyInjectionBinder
    {
        public static void Bind(IServiceCollection services)
        {
            services.AddScoped<ILibraryRepository, LibraryRepository>();
        }
    }
}