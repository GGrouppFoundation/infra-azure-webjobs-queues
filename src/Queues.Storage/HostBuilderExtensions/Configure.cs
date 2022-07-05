using System;
using GGroupp.Infra;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

partial class QueueItemHandlerHostBuilderExtensions
{
    public static IHostBuilder ConfigureQueueProcessor(
        this IHostBuilder hostBuilder,
        Func<IServiceProvider, IQueueItemHandler> queueItemHandlerResolver)
    {
        _ = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));
        _ = queueItemHandlerResolver ?? throw new ArgumentNullException(nameof(queueItemHandlerResolver));

        return hostBuilder.InternalConfigureQueueProcessor(queueItemHandlerResolver);
    }

    internal static IHostBuilder InternalConfigureQueueProcessor(
        this IHostBuilder hostBuilder,
        Func<IServiceProvider, IQueueItemHandler> queueItemHandlerResolver)
    {
        return hostBuilder.ConfigureServices(ConfigureServices).ConfigureWebJobs(ConfigureWebJobs);

        void ConfigureServices(IServiceCollection services)
            =>
            services.AddSingleton(ResolveHandler).AddTypeLocator<QueueItemHandlerFunction>();

        IQueueMessageHandler ResolveHandler(IServiceProvider serviceProvider)
            =>
            QueueMessageHandler.Create(
                queueItemHandlerResolver.Invoke(serviceProvider),
                serviceProvider.GetLoggerFactory());
    }
}