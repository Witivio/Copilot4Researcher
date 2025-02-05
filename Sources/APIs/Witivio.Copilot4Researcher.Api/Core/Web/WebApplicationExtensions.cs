public static class WebApplicationExtensions
{
    /// <summary>
    /// Configures the middleware pipeline for the web application
    /// </summary>
    /// <param name="app">The WebApplication instance to configure</param>
    public static void UseMiddleware(this WebApplication app)
    {
        // Enable Swagger and SwaggerUI only in development environment
        if (app.Environment.IsDevelopment())
        {
            // Enables Swagger middleware to serve the OpenAPI specification
            app.UseSwagger();
            
            // Enables SwaggerUI middleware to serve the interactive API documentation
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Copilot for Researcher APIs");
            });
        }

        // Redirects HTTP requests to HTTPS
        app.UseHttpsRedirection();
        
        // Enables authorization capabilities
        app.UseAuthorization();
        
        // Adds endpoints for controller actions without specifying explicit routes
        app.MapControllers();
    }
}