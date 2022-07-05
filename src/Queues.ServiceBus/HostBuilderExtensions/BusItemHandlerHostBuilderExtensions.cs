using System;
using GGroupp.Infra;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting;

public static partial class BusItemHandlerHostBuilderExtensions
{
    private const int DefaultMaxRetries = 10;

    private static void ConfigureWebJobs(HostBuilderContext context, IWebJobsBuilder builder)
        =>
        builder.AddAzureStorageCoreServices().AddExecutionContextBinding().AddServiceBus(context.ConfigureQueueOptions);

    private static IServiceCollection AddTypeLocator<TFunction>(this IServiceCollection services)
    {
        return services.AddSingleton(ResolveTypeLocator);

        static ITypeLocator ResolveTypeLocator(IServiceProvider _)
            =>
            new BusHandlerTypeLocator<TFunction>();
    }

    private static void ConfigureQueueOptions(this HostBuilderContext context, ServiceBusOptions busOptions)
    {
        var options = context.Configuration.GetBusQueueOptionJson();
        if (options is null)
        {
            return;
        }

        if (options.PrefetchCount is not null)
        {
            busOptions.PrefetchCount = options.PrefetchCount.Value;
        }

        if (options.Retry is not null && busOptions.ClientRetryOptions is null)
        {
            busOptions.ClientRetryOptions = new();
        }

        if (options.Retry?.Mode is not null)
        {
            busOptions.ClientRetryOptions.Mode = options.Retry.Mode.Value;
        }

        if (options.Retry?.MaxRetries is not null)
        {
            busOptions.ClientRetryOptions.MaxRetries = options.Retry.MaxRetries.Value;
        }

        if (options.Retry?.Delay is not null)
        {
            busOptions.ClientRetryOptions.Delay = options.Retry.Delay.Value;
        }

        if (options.Retry?.MaxDelay is not null)
        {
            busOptions.ClientRetryOptions.MaxDelay = options.Retry.MaxDelay.Value;
        }

        if (options.Retry?.TryTimeout is not null)
        {
            busOptions.ClientRetryOptions.TryTimeout = options.Retry.TryTimeout.Value;
        }

        if (options.TransportType is not null)
        {
            busOptions.TransportType = options.TransportType.Value;
        }

        if (options.AutoCompleteMessages is not null)
        {
            busOptions.AutoCompleteMessages = options.AutoCompleteMessages.Value;
        }

        if (options.MaxAutoLockRenewalDuration is not null)
        {
            busOptions.MaxAutoLockRenewalDuration = options.MaxAutoLockRenewalDuration.Value;
        }

        if (options.MaxConcurrentCalls is not null)
        {
            busOptions.MaxConcurrentCalls = options.MaxConcurrentCalls.Value;
        }

        if (options.MaxMessageBatchSize is not null)
        {
            busOptions.MaxMessageBatchSize = options.MaxMessageBatchSize.Value;
        }

        if (options.SessionIdleTimeout is not null)
        {
            busOptions.SessionIdleTimeout = options.SessionIdleTimeout.Value;
        }

        if (options.EnableCrossEntityTransactions is not null)
        {
            busOptions.EnableCrossEntityTransactions = options.EnableCrossEntityTransactions.Value;
        }
    }

    private static int GetMaxRetries(this IServiceProvider serviceProvider)
        =>
        serviceProvider.GetRequiredService<IConfiguration>().GetBusQueueOptionJson()?.Retry?.MaxRetries ?? DefaultMaxRetries;

    private static BusQueueOptionJson? GetBusQueueOptionJson(this IConfiguration configuration)
        =>
        configuration.GetSection("BusQueue").Get<BusQueueOptionJson?>();

    private static ILoggerFactory GetLoggerFactory(this IServiceProvider serviceProvider)
        =>
        serviceProvider.GetRequiredService<ILoggerFactory>();
}