using Microsoft.Extensions.DependencyInjection;
using NinjaTurtles.Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Core.DependencyResolver
{
    public class CoreModule: ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddSingleton<Stopwatch>();
        }
    }
}
