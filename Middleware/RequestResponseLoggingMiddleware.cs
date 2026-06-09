using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace UserManagementApi.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            _logger.LogInformation("HTTP Request: {Method} {Path} received.", request.Method, request.Path);

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var response = context.Response;
                _logger.LogInformation("HTTP Response: {Method} {Path} responded {StatusCode} in {ElapsedMs} ms.",
                    request.Method, request.Path, response.StatusCode, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
