using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Retry;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace Witivio.Copilot4Researcher.Core
{
    public static class ResilienceExtensions
    {
        public static IHttpStandardResiliencePipelineBuilder AddResilienceHandler(this IHttpClientBuilder builder)
        {
            //return builder.AddStandardResilienceHandler(options =>
            // {
            //     //options.AttemptTimeout = new HttpTimeoutStrategyOptions { Timeout = TimeSpan.FromSeconds(8) };
            //     options.Retry.MaxRetryAttempts = 5; // Retry 3 times
            //     options.Retry.ShouldRetryAfterHeader = true;
            //     options.Retry.ShouldHandle = e => ValueTask.FromResult(e.Outcome.Result?.StatusCode == HttpStatusCode.InternalServerError || e.Outcome.Result?.StatusCode == HttpStatusCode.BadRequest || e.Outcome.Result?.StatusCode == HttpStatusCode.TooManyRequests);
            // });

            return builder.AddStandardResilienceHandler();
        }

        public static IHttpResiliencePipelineBuilder AddResiliencePubmedApiKeyHandler(this IHttpClientBuilder builder, IConfiguration configuration)
        {
            var handler = builder.AddResilienceHandler("apiKey", builder =>
            {
                builder.AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromSeconds(1),
                    BackoffType = DelayBackoffType.Exponential,
                    OnRetry = async args =>
                    {
                        // Logic to change the API key in the query string
                        var request = args.Outcome.Result?.RequestMessage;
                        if (request != null)
                        {
                            var apiKeys = configuration.GetSection("PubmedApiKeys").Get<string[]>();
                            var currentKeyIndex = new Random().Next(apiKeys.Length);
                            var newApiKey = apiKeys[currentKeyIndex];
                            Debug.WriteLine($"\u001b[31m{newApiKey}\u001b[0m");
                            var uriBuilder = new UriBuilder(request.RequestUri);
                            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
                            query["api_key"] = newApiKey;
                            uriBuilder.Query = query.ToString();
                            request.RequestUri = uriBuilder.Uri;
                        }
                        await Task.CompletedTask;
                    }
                });
            });
            return handler;
        }
    }
}
