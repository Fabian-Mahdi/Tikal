using OpenTelemetry.Resources;

namespace IdentityAPI.Extensions;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder ConfigureOpenTelemetry(this ILoggingBuilder builder)
    {
        builder.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddService("IdentityAPI"));

            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;
        });

        return builder;
    }
}