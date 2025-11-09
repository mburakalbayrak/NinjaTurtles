using System.Net;
using Serilog;
using System.Text.Json;

namespace NinjaTurtles.WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;
                Log.Error(ex, "Unhandled exception for {Method} {Path} TraceId={TraceId}", context.Request.Method, context.Request.Path, traceId);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var payload = new
                {
                    success = false,
                    message = ex.Message,
                    traceId,
                    innerException = ex.InnerException?.Message
                };
                var json = JsonSerializer.Serialize(payload);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
