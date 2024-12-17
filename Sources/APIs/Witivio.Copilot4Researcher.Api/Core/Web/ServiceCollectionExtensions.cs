using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Witivio.Copilot4Researcher.Core;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.EntityFrameworkCore;
using Witivio.Copilot4Researcher.Features.ClinicalTrials;
using Witivio.Copilot4Researcher.Features.Literature.BioRxiv;
using Witivio.Copilot4Researcher.Features.Literature.HAL;
using Witivio.Copilot4Researcher.Features.Literature.Scimago;
using Witivio.Copilot4Researcher.Features.Literature.Pubmed;
using Witivio.Copilot4Researcher.Features.Collaboration;
using Witivio.Copilot4Researcher.Features.Gene.ProteinAtlas;
using Witivio.Copilot4Researcher.Features.Patents;
using Witivio.Copilot4Researcher.Features.TextRewrite;
using Witivio.Copilot4Researcher.Api.Options;
using Witivio.Copilot4Researcher.Api.Core.Options;
using Witivio.Copilot4Researcher.Api.Features.Collaboration;

/// <summary>
/// Extension methods for configuring services in the application
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures API endpoints, Swagger documentation, and related services
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The IServiceCollection for chaining</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Copilot for Researcher APIs",
                Description = "APIs for Copilot for Researcher"
            });
            options.EnableAnnotations();
        });

        return services;
    }

    /// <summary>
    /// Configures core infrastructure services including Azure OpenAI configuration,
    /// database context, chat completion services, Microsoft Graph client, and background services
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to</param>
    /// <param name="configuration">The application configuration containing connection strings and service settings</param>
    /// <returns>The IServiceCollection for chaining</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add options configuration
        services.Configure<AzureOpenAIOptions>(
            configuration.GetSection(AzureOpenAIOptions.SectionName));

        services.Configure<SharePointOptions>(
          configuration.GetSection(SharePointOptions.SectionName));

        services.AddDbContext<DeliveryNoteContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IChatCompletionService>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;
            return new AzureOpenAIChatCompletionService(
                options.ChatDeploymentName,
                options.Endpoint,
                options.ApiKey);
        });

        services.AddScoped<GraphServiceClient>(_ =>
            new GraphServiceClient(new DefaultAzureCredential()));

        services.AddKernel();
        services.AddHostedService<DeliveryScanBackgroundService>();

        return services;
    }

    /// <summary>
    /// Configures external API clients and domain-specific services with resilience patterns
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to</param>
    /// <param name="configuration">The application configuration containing API keys and endpoints for external services</param>
    /// <returns>The IServiceCollection for chaining</returns>
    /// <remarks>
    /// This method configures:
    /// - Research database clients (Pubmed, BioRxiv, ProteinAtlas, HAL, Patents, ClinicalTrials)
    /// - Domain services for rewrite rules, journal data, delivery notes, and user management
    /// All HTTP clients are configured with appropriate resilience patterns
    /// </remarks>
    public static IServiceCollection AddExternalClients(this IServiceCollection services, IConfiguration configuration)
    {
        // HTTP clients
        services.AddHttpClient<IPubmedClient, PubmedClient>()
            .AddResiliencePubmedApiKeyHandler(configuration);
        services.AddHttpClient<IBioRxivClient, BioRxivClient>()
            .AddResilienceHandler();
        services.AddHttpClient<IProteinAtlasClient, ProteinAtlasClient>()
            .AddResilienceHandler();
        services.AddHttpClient<IHALClient, HALCLient>()
            .AddResilienceHandler();
        services.AddHttpClient<IPatentsClient, PatentsClient>()
            .AddResilienceHandler();
        services.AddHttpClient<IClinicalTrialsClient, ClinicalTrialsClient>()
            .AddResilienceHandler();

        // Services
        services.AddScoped<IRewriteRulesService, RewriteRulesSharePointService>();
        services.AddSingleton<IJournalDataService, JournalDataService>();
        services.AddScoped<IDeliveryNoteService, DeliveryNoteService>();
        services.AddScoped<IEntraIDUserService, EntraIDUserService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICurieDirectoryService, CurieDirectoryService>();
        services.AddScoped<ISharePointLogService, SharePointLogService>();

        return services;
    }
}
