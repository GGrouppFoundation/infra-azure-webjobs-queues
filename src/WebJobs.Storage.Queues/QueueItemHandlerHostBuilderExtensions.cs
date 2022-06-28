using System;
using GGroupp.Infra;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class QueueItemHandlerHostBuilderExtensions
{
    public static IHostBuilder ConfigureQueueProcessor(
        this IHostBuilder hostBuilder, Func<IServiceProvider, IQueueItemHandler> queueItemHandlerResolver)
    {
        _ = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));
        _ = queueItemHandlerResolver ?? throw new ArgumentNullException(nameof(queueItemHandlerResolver));

        return hostBuilder.ConfigureServices(ConfigureServices).ConfigureWebJobs(ConfigureWebJobs);

        void ConfigureServices(IServiceCollection services)
            =>
            services.ConfigureQueueProcessorServices(queueItemHandlerResolver);
    }

    private static void ConfigureQueueProcessorServices(
        this IServiceCollection services, Func<IServiceProvider, IQueueItemHandler> queueItemHandlerResolver)
    {
        _ = services.AddSingleton(ResolveNameResolver).AddSingleton(queueItemHandlerResolver).AddSingleton(ResolveTypeLocator);

        INameResolver ResolveNameResolver(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return ConfigurationNameResolver.Create(configuration);
        }

        ITypeLocator ResolveTypeLocator(IServiceProvider _)
            =>
            new QueueItemProcessorTypeLocator();
    }

    private static void ConfigureWebJobs(HostBuilderContext context, IWebJobsBuilder builder)
        =>
        builder.AddAzureStorageCoreServices().AddAzureStorageQueues(context.ConfigureQueueOptions);

    private static void ConfigureQueueOptions(this HostBuilderContext context, QueuesOptions queuesOptions)
    {
        var options = context.Configuration.GetSection("QueueIn").Get<QueueOptionJson?>();
        if (options is null)
        {
            return;
        }

        if (options.BatchSize is not null)
        {
            queuesOptions.BatchSize = options.BatchSize.Value;
        }

        if (options.NewBatchThreshold is not null)
        {
            queuesOptions.NewBatchThreshold = options.NewBatchThreshold.Value;
        }

        if (options.MaxPollingInterval is not null)
        {
            queuesOptions.MaxPollingInterval = options.MaxPollingInterval.Value;
        }

        if (options.MaxDequeueCount is not null)
        {
            queuesOptions.MaxDequeueCount = options.MaxDequeueCount.Value;
        }

        if (options.VisibilityTimeout is not null)
        {
            queuesOptions.VisibilityTimeout = options.VisibilityTimeout.Value;
        }

        if (options.MessageEncoding is not null)
        {
            queuesOptions.MessageEncoding = options.MessageEncoding.Value;
        }
    }
}