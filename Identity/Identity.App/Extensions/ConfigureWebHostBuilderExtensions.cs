using Sentry.OpenTelemetry;

namespace Identity.App.Extensions;

public static class ConfigureWebHostBuilderExtensions
{
    public static void AddSentry(this ConfigureWebHostBuilder builder)
    {
        builder.UseSentry(options =>
        {
            options.UseOpenTelemetry();
            options.SetBeforeSendTransaction(sentryTransaction =>
            {
                // preflight options requests
                if (sentryTransaction.Request.Method == "OPTIONS")
                {
                    return null;
                }

                // health checks
                if (sentryTransaction.Request.Url?.Contains("healthcheck") ?? false)
                {
                    return null;
                }

                // app service always on pings
                if (sentryTransaction.Request.Headers.TryGetValue("User-Agent", out string? agent) &&
                    agent == "AlwaysOn")
                {
                    return null;
                }

                return sentryTransaction;
            });
        });
    }
}