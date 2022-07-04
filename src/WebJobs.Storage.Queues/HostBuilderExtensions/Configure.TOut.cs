using System;
using GGroupp.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

partial class QueueItemHandlerHostBuilderExtensions
{
    public static IHostBuilder ConfigureQueueProcessor<TIn, TOut>(
        this IHostBuilder hostBuilder,
        Func<IServiceProvider, IQueueItemHandler<TIn, TOut>> queueItemHandlerResolver)
        where TIn : notnull
    {
        _ = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));
        _ = queueItemHandlerResolver ?? throw new ArgumentNullException(nameof(queueItemHandlerResolver));

        return hostBuilder.InternalConfigureQueueProcessor(queueItemHandlerResolver);
    }

    internal static IHostBuilder InternalConfigureQueueProcessor<TIn, TOut>(
        this IHostBuilder hostBuilder,
        Func<IServiceProvider, IQueueItemHandler<TIn, TOut>> queueItemHandlerResolver)
        where TIn : notnull
    {
        return hostBuilder.ConfigureServices(ConfigureServices).ConfigureWebJobs(ConfigureWebJobs);

        void ConfigureServices(IServiceCollection services)
            =>
            services.AddSingleton(ResolveHandler).AddTypeLocator<QueueItemHandlerOutFunction>();

        IQueueMessageHandler ResolveHandler(IServiceProvider serviceProvider)
            =>
            QueueMessageHandler<TIn, TOut>.Create(
                queueItemHandlerResolver.Invoke(serviceProvider),
                serviceProvider.GetLoggerFactory(),
                isOutSerialized: true);
    }
}