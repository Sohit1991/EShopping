using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging.Correlation
{
    public static class AddBuilderExtension
    {
        public static IApplicationBuilder AddCorrelationIdMiddleware(this IApplicationBuilder applicationBuilder) 
            =>applicationBuilder.UseMiddleware<CorrelationIdMiddleware>();
            
    }
}
