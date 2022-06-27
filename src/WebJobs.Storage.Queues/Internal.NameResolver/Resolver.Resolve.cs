using System;
using Microsoft.Extensions.Configuration;

namespace GGroupp.Infra;

partial class ConfigurationNameResolver
{
    public string Resolve(string name)
    {
        var value = configuration.GetValue(name, string.Empty);

        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Configuration '{name}' value must be specified");
        }

        return value;
    }
}