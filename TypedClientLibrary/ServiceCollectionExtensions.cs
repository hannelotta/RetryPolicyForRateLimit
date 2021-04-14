using Microsoft.Extensions.DependencyInjection;
using System;

namespace TypedClientLibrary
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureRetryPolicy(this IServiceCollection serviceCollection,
            Action<IHttpClientBuilder> configure)
        {
            IHttpClientBuilder builder = serviceCollection.AddHttpClient<ITypedClient, TypedHttpClient>();
            configure.Invoke(builder);
        }
    }
}
