using Azure.Identity;

namespace Tikal.App.Extensions;

public static class ConfigurationManagerExtensions
{
    public static void ConfigureKeyVault(this ConfigurationManager configuration)
    {
        string keyVaultName = configuration.GetValue<string>("KeyVaultName") ?? string.Empty;

        Uri keyVaultUri = new($"https://{keyVaultName}.vault.azure.net/");

        configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
    }
}