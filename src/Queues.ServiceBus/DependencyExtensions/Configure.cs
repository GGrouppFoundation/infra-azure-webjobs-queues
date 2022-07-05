using System;
using Microsoft.Extensions.Hosting;
using PrimeFuncPack;

namespace GGroupp.Infra;

partial class BusItemHandlerDependencyExtensions
{
    public static IHostBuilder ConfigureBusQueueProcessor(this Dependency<IQueueItemHandler> dependency, IHostBuilder hostBuilder)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));
        _ = hostBuilder ?? throw new ArgumentNullException(nameof(hostBuilder));

        return hostBuilder.InternalConfigureBusQueueProcessor(dependency.Resolve);
    }
}