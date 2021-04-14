using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RateLimitedApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Needed to load configuration from appsettings.json
            services.AddOptions();

            // Needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            // Load general configuration from appsettings.json
            services.Configure<ClientRateLimitOptions>(Configuration.GetSection("ClientRateLimiting"));

            // Load client rules from appsettings.json
            services.Configure<ClientRateLimitPolicies>(Configuration.GetSection("ClientRateLimitPolicies"));

            // Inject counter and rules stores
            services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // Add framework services.
            services.AddMvc();

            // https://github.com/aspnet/Hosting/issues/793
            // The IHttpContextAccessor service is not registered by default.
            // The clientId/clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseClientRateLimiting();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}