var builder = WebApplication.CreateBuilder(args);



// Group related services configuration
builder.Services.AddEndpoints(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddExternalClients(builder.Configuration);

// Add health checks
builder.Services.AddHealthChecks();

// Configure configuration sources in a cleaner way
builder.ConfigureAppConfiguration();

var app = builder.Build();


// Configure middleware pipeline
app.UseMiddleware();

// Map health check endpoint
app.MapHealthChecks("/health");

app.Run();

// Keep this for testing purposes
public partial class Program { }
