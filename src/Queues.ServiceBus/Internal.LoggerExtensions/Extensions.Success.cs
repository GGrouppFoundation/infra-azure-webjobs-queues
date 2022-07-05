using System;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

partial class BusHandlerLoggerExtensions
{
    internal static Unit LogSuccess(this ILogger logger, string messageId)
    {
        logger.LogInformation("Service bus queue message '{id}' was proccessed successfully", messageId);
        return default;
    }
}