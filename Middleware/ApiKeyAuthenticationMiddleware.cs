using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserManagementApi.Middleware
{
    public class ApiKeyAuthenticationMiddleware
    {
        private const string ApiKeyHeaderName = "X-API-Key";
        private readonly RequestDelegate _next;

        public ApiKeyAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? "";

            // Bypass authentication for OpenAPI and Swagger endpoints
            if (path.StartsWith("/openapi") || path.StartsWith("/swagger") || path.Equals("/favicon.ico"))
            {
                await _next(context);
                return;
            }

            var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
            var expectedApiKey = configuration.GetValue<string>("Authentication:ApiKey") ?? "SecUserApi2026";

            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = "Unauthorized. API Key is missing. Please provide X-API-Key header." });
                return;
            }

            if (extractedApiKey != expectedApiKey)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = "Unauthorized. Invalid API Key." });
                return;
            }

            await _next(context);
        }
    }
}
