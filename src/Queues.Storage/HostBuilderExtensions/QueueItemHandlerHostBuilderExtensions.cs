using System;
using GGroupp.Infra;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting;

public static partial class QueueItemHandlerHostBuilderExtensions
{
    private static void ConfigureWebJobs(HostBuilderContext context, IWebJobsBuilder builder)
        =>
        builder.AddAzureStorageCoreServices().AddExecutionContextBinding().AddAzureStorageQueues(context.ConfigureQueueOptions);

    private static IServiceCollection AddTypeLocator<TFunction>(this IServiceCollection services)
    {
        return services.AddSingleton(ResolveTypeLocator);

        static ITypeLocator ResolveTypeLocator(IServiceProvider _)
            =>
            new HandlerTypeLocator<TFunction>();
    }

    private static void ConfigureQueueOptions(this HostBuilderContext context, QueuesOptions queuesOptions)
    {
        var options = context.Configuration.GetSection("Queue").Get<QueueOptionJson?>();
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

    private static ILoggerFactory GetLoggerFactory(this IServiceProvider serviceProvider)
        =>
        serviceProvider.GetRequiredService<ILoggerFactory>();
}