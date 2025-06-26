using Azure.Identity;

namespace IdentityAPI.Extensions;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager ConfigureKeyVault(this ConfigurationManager configuration)
    {
        string keyVaultName = configuration.GetValue<string>("keyVaultName") ?? string.Empty;

        Uri keyVaultUri = new($"https://{keyVaultName}.vault.azure.net/");

        configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

        return configuration;
    }
}