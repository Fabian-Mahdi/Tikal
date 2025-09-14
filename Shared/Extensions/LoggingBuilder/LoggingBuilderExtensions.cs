using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;

namespace Shared.Extensions.LoggingBuilder;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder ConfigureOpenTelemetry(this ILoggingBuilder builder, string serviceName)
    {
        builder.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddService(serviceName));

            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;
        });

        return builder;
    }
}