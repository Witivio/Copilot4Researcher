using Azure.Identity;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the application's configuration sources including JSON files, environment variables, 
    /// user secrets, and Azure Key Vault (if configured)
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder to configure</param>
    public static void ConfigureAppConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>(optional: true);

        // Configure Azure Key Vault if URL is provided in configuration
        var keyVaultUrl = builder.Configuration["KeyVault:Url"];
        if (!string.IsNullOrEmpty(keyVaultUrl))
        {
            builder.Configuration.AddAzureKeyVault(
                new Uri(keyVaultUrl),
                // Uses DefaultAzureCredential which attempts multiple authentication methods
                new DefaultAzureCredential());
        }
    }
}
