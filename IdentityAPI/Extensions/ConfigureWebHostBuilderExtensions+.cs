using Sentry.OpenTelemetry;

namespace IdentityAPI.Extensions;

public static class ConfigureWebHostBuilderExtensions_
{
    public static void AddProdSentry(this ConfigureWebHostBuilder builder)
    {
        builder.UseSentry(options =>
        {
            options.UseOpenTelemetry();
            options.SetBeforeSendTransaction(sentryTransaction =>
            {
                if (sentryTransaction.Request.Method == "OPTIONS")
                {
                    return null;
                }

                if (sentryTransaction.Request.Url?.Contains("healthcheck") ?? false)
                {
                    return null;
                }

                return sentryTransaction;
            });
        });
    }
}