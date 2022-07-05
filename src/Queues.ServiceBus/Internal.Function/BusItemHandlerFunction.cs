using System;

namespace GGroupp.Infra;

internal sealed partial class BusItemHandlerFunction
{
    private readonly IBusMessageHandler busMessageHandler;

    public BusItemHandlerFunction(IBusMessageHandler busMessageHandler)
        =>
        this.busMessageHandler = busMessageHandler ?? throw new ArgumentNullException(nameof(busMessageHandler));
}