using Common.Logging.Correlation;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Behaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicatonService(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnHandledExcepionBehaviour<,>));

            return services;
        }
    }
}
