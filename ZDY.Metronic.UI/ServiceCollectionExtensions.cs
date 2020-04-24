using System;
using System.Collections.Generic;
using System.Text;
using ZDY.Metronic.UI;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMetronicUI(this IServiceCollection services, Action<Settings> configure = null)
        {
            configure?.Invoke(Settings.GetInstance());

            services.AddSingleton<IMetronicUI, MetronicUI>();
        }
    }
}
