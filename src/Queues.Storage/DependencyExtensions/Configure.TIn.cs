using System;
using GGroupp.Infra;
using Microsoft.Extensions.Hosting;
using PrimeFuncPack;

namespace GGroupp.Infra;

partial class QueueItemHandlerDependencyExtensions
{
    public static IHostBuilder ConfigureQueueProcessor<TIn>(this Dependency<IQueueItemHandler<TIn, Unit>> dependency, IHostBuilder hostBuilder)
        where TIn : notnull
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));

        return hostBuilder.InternalConfigureQueueProcessor(dependency.Resolve);
    }
}