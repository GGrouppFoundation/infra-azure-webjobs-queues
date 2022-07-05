using System;
using GGroupp.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

partial class BusItemHandlerHostBuilderExtensions
{
    public static IHostBuilder ConfigureBusQueueProcessor(
        this IHostBuilder hostBuilder,
        Func<IServiceProvider, IQueueItemHandler> queueItemHandlerResolver)
    {
        _ = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));
        _ = queueItemHandlerResolver ?? throw new ArgumentNullException(nameof(queueItemHandlerResolver));

        return hostBuilder.InternalConfigureBusQueueProcessor(queueItemHandlerResolver);
    }

    internal static IHostBuilder InternalConfigureBusQueueProcessor(
        this IHostBuilder hostBuilder,
        Func<IServiceProvider, IQueueItemHandler> queueItemHandlerResolver)
    {
        return hostBuilder.ConfigureServices(ConfigureServices).ConfigureWebJobs(ConfigureWebJobs);

        void ConfigureServices(IServiceCollection services)
            =>
            services.AddSingleton(ResolveHandler).AddTypeLocator<BusItemHandlerFunction>();

        IBusMessageHandler ResolveHandler(IServiceProvider serviceProvider)
            =>
            BusMessageHandler.Create(
                queueItemHandlerResolver.Invoke(serviceProvider),
                serviceProvider.GetLoggerFactory(),
                serviceProvider.GetMaxRetries());
    }
}