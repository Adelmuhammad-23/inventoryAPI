using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IMS.Application
{
    public static class ModuleApplicationDependencis
    {
        public static IServiceCollection AddApplicationDependencis(this IServiceCollection services)
        {
            //// Configuration of Mediator
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            //Configuration of AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
