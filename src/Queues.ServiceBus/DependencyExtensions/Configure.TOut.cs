using System;
using Microsoft.Extensions.Hosting;
using PrimeFuncPack;

namespace GGroupp.Infra;

partial class BusItemHandlerDependencyExtensions
{
    public static IHostBuilder ConfigureBusQueueProcessor<TIn, TOut>(this Dependency<IQueueItemHandler<TIn, TOut>> dependency, IHostBuilder hostBuilder)
        where TIn : notnull
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));

        return hostBuilder.InternalConfigureBusQueueProcessor(dependency.Resolve);
    }
}