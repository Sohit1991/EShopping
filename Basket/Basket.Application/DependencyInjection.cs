using Microsoft.Extensions.DependencyInjection;

namespace Basket.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDepedency(this IServiceCollection services)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            return services;
        }
    }
}
