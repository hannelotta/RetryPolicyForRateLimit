using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TypedClientLibrary;

IAsyncPolicy<HttpResponseMessage> retryPolicy = Policy
    .HandleResult<HttpResponseMessage>(response => response.StatusCode == HttpStatusCode.TooManyRequests)
    .WaitAndRetryForeverAsync(
            sleepDurationProvider: (retryCount, response, context) => {
                TimeSpan? retryAfter = response.Result.Headers.RetryAfter.Delta;
                Console.WriteLine("Retry-After header value from the HTTP response is " + retryAfter);
                Console.WriteLine();

                // Uncomment the following line to test the actual rate limit response
                return retryAfter ?? TimeSpan.FromSeconds(2);

                // Uncomment the following line to test the retry policy when the request gets rate limited
                //return TimeSpan.FromSeconds(2);
            },
            onRetryAsync: async (response, retryCount, retryAfter, context) => {
                Console.WriteLine("Retry count: " + retryCount);
                Console.WriteLine();
                await Task.CompletedTask;
            });

var serviceCollection = new ServiceCollection();

serviceCollection.ConfigureRetryPolicy(builder =>
{
    builder.AddPolicyHandler(retryPolicy);
});

IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
var client = serviceProvider.GetService<ITypedClient>();

// Call the rate limited API service in a loop
while (true)
{
    await Task.Delay(TimeSpan.FromMilliseconds(500));

    HttpResponseMessage response = await client.TestRateLimitedService(CancellationToken.None);

    Console.WriteLine("HTTP response (outside retry policy): " + response.StatusCode);
    Console.WriteLine();
}