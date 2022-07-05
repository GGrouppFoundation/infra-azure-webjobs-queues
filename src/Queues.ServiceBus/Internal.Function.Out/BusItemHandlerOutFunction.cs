using System;

namespace GGroupp.Infra;

internal sealed partial class BusItemHandlerOutFunction
{
    private readonly IBusMessageHandler busMessageHandler;

    public BusItemHandlerOutFunction(IBusMessageHandler busMessageHandler)
        =>
        this.busMessageHandler = busMessageHandler ?? throw new ArgumentNullException(nameof(busMessageHandler));
}