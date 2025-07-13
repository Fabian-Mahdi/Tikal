﻿using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace IdentityAPI.Extensions;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder ConfigureDevOpenTelemetry(this ILoggingBuilder builder, IConfiguration configuration)
    {
        IConfigurationSection openTelemetryConfig = configuration.GetSection("Logging:OpenTelemetry");

        builder.AddOpenTelemetry(configure =>
        {
            configure.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddService("IdentityAPI"));

            configure.IncludeFormattedMessage = true;
            configure.IncludeScopes = true;

            configure.AddOtlpExporter(otlpExporterOptions =>
            {
                otlpExporterOptions.Endpoint = new Uri(openTelemetryConfig.GetValue<string>("location") ?? "");
                otlpExporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                otlpExporterOptions.Headers = $"X-Seq-ApiKey={openTelemetryConfig.GetValue<string>("ApiKey")}";
            });
        });

        return builder;
    }
}