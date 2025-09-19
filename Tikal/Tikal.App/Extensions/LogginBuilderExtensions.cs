using OpenTelemetry.Resources;

namespace Tikal.App.Extensions;

public static class LoggingBuilderExtensions
{
    public static void ConfigureOpenTelemetry(this ILoggingBuilder builder)
    {
        builder.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddService("TikalBackend"));

            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;
        });
    }
}