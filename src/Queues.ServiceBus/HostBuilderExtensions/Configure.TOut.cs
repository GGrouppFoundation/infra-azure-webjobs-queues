using System;
using GGroupp.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

partial class BusItemHandlerHostBuilderExtensions
{
    public static IHostBuilder ConfigureBusQueueProcessor<TIn, TOut>(
        this IHostBuilder hostBuilder,
        Func<IServiceProvider, IQueueItemHandler<TIn, TOut>> queueItemHandlerResolver)
        where TIn : notnull
    {
        _ = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));
        _ = queueItemHandlerResolver ?? throw new ArgumentNullException(nameof(queueItemHandlerResolver));

        return hostBuilder.InternalConfigureBusQueueProcessor(queueItemHandlerResolver);
    }

    internal static IHostBuilder InternalConfigureBusQueueProcessor<TIn, TOut>(
        this IHostBuilder hostBuilder,
        Func<IServiceProvider, IQueueItemHandler<TIn, TOut>> queueItemHandlerResolver)
        where TIn : notnull
    {
        return hostBuilder.ConfigureServices(ConfigureServices).ConfigureWebJobs(ConfigureWebJobs);

        void ConfigureServices(IServiceCollection services)
            =>
            services.AddSingleton(ResolveHandler).AddTypeLocator<BusItemHandlerOutFunction>();

        IBusMessageHandler ResolveHandler(IServiceProvider serviceProvider)
            =>
            BusMessageHandler<TIn, TOut>.Create(
                queueItemHandlerResolver.Invoke(serviceProvider),
                serviceProvider.GetLoggerFactory(),
                maxDeliveryCount: serviceProvider.GetMaxRetries(),
                isOutSerializable: true);
    }
}