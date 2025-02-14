using IMS.Domain.Interfaces;
using IMS.Domain.UnitOfWorkInterface;
using IMS.Infrastructure.Repositories;
using IMS.Infrastructure.UnitOfWorkRepo;
using Microsoft.Extensions.DependencyInjection;


namespace IMS.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencis(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
