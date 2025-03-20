using CarsAPI.Interfaces;
using CarsAPI.Repositories;
using CarsAPI.Services;

namespace CarsAPI.Startup
{
    public static class DependenciesConfig
    {
        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            services.AddScoped<ICarRepository, CarRepository>();

            services.AddScoped<ICarSearchService, CarSearchService>();


        }
    }
}
