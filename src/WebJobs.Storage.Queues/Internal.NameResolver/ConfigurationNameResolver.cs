using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace GGroupp.Infra;

internal sealed partial class ConfigurationNameResolver : INameResolver
{
    public static ConfigurationNameResolver Create(IConfiguration configuration)
        =>
        new(
            configuration ?? throw new ArgumentNullException(nameof(configuration)));

    private readonly IConfiguration configuration;

    private ConfigurationNameResolver(IConfiguration configuration)
        =>
        this.configuration = configuration;
}