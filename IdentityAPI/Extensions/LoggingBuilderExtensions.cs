using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace IdentityAPI.Extensions;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder ConfigureOpenTelemetry(this ILoggingBuilder builder, IConfiguration configuration)
    {
        IConfigurationSection openTelemetryConfig = configuration.GetSection("Logging:OpenTelemetry");

        builder.AddOpenTelemetry(configure =>
        {
            configure.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddService("IdentityAPI"));

            configure.IncludeFormattedMessage = true;
            configure.IncludeScopes = true;

            configure.AddOtlpExporter(configure =>
            {
                configure.Endpoint = new Uri(openTelemetryConfig.GetValue<string>("location") ?? "");
                configure.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                configure.Headers = $"X-Seq-ApiKey={openTelemetryConfig.GetValue<string>("ApiKey")}";
            });
        });

        return builder;
    }
}