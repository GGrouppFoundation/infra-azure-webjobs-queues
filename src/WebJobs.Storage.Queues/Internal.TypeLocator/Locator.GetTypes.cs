using System;
using System.Collections.Generic;

namespace GGroupp.Infra;

partial class QueueItemProcessorTypeLocator
{
    public IReadOnlyList<Type> GetTypes() => new[] { typeof(QueueItemProcessorFunction) };
}