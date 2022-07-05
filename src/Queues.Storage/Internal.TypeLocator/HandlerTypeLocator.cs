using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;

namespace GGroupp.Infra;

internal sealed class HandlerTypeLocator<TFunction> : ITypeLocator
{
    public IReadOnlyList<Type> GetTypes() => new[] { typeof(TFunction) };
}