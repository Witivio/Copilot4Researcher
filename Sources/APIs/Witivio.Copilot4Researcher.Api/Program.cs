using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using System.Net;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Witivio.Copilot4Researcher.Core;
using Witivio.Copilot4Researcher.Providers.BioRxiv;
using Witivio.Copilot4Researcher.Providers.ProteinAtlas;
using Witivio.Copilot4Researcher.Providers.Pubmed;
using Witivio.Copilot4Researcher.Providers.Patents;
using Witivio.Copilot4Researcher.Providers.ClinicalTrials;
using Witivio.Copilot4Researcher.Providers.Scimago;
using Azure.Identity;
using Microsoft.Graph;
using Azure.Core;
using Witivio.Copilot4Researcher.Api.Providers;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Copilot for Reseracher APIs",
        Description = "An API to search literature through PubMed",
    });

    // Add server information
    options.AddServer(new OpenApiServer
    {
        Url = "https://localhost:7068/",
        Description = "Local Development Server"
    });

    options.AddServer(new OpenApiServer
    {
        Url = "https://e1ed1a7105d2.ngrok.app",
        Description = "Production Server"
    });

    options.EnableAnnotations();
});

// Add configuration sources
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true);

// Add Azure Key Vault if configured
var keyVaultUrl = builder.Configuration["KeyVault:Url"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl),
        new DefaultAzureCredential());
}


builder.Services.AddHttpClient<IPubmedClient, PubmedClient>().AddResiliencePubmedApiKeyHandler(builder.Configuration);
builder.Services.AddHttpClient<IBioRxivClient, BioRxivClient>().AddResilienceHandler();
builder.Services.AddHttpClient<IProteinAtlasClient, ProteinAtlasClient>().AddResilienceHandler();
builder.Services.AddHttpClient<IHALClient, HALCLient>().AddResilienceHandler();
builder.Services.AddHttpClient<IPatentsClient, PatentsClient>().AddResilienceHandler();
builder.Services.AddHttpClient<IClinicalTrialsClient, ClinicalTrialsClient>().AddResilienceHandler();

builder.Services.AddSingleton<IJournalDataService, JournalDataService>();

builder.Services.AddSingleton<TokenCredential, DefaultAzureCredential>();

// https://learn.microsoft.com/en-us/azure/app-service/scenario-secure-app-access-microsoft-graph-as-app?tabs=azure-powershell

builder.Services.AddScoped<GraphServiceClient>(sp =>
{
    //var managedIdentityCredential = new ManagedIdentityCredential();

    //var chainedTokenCredential = new ChainedTokenCredential(
    //    managedIdentityCredential,
    //    new EnvironmentCredential()
    //);

    var credential = sp.GetRequiredService<TokenCredential>();

    return new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });

});


builder.Services.AddScoped<IRewriteRulesService, RewriteRulesSharePointService>(); 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Copilot for Reseracher APIs");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();




public partial class Program { }