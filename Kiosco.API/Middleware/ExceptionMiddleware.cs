
using System.Net;
using System.Text.Json;

namespace Kiosco.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Ocurrió un error inesperado.");

                if (context.Response.HasStarted)
                    throw;

                context.Response.Clear();

                context.Response.ContentType =
                    "application/json";

                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                var respuesta = new
                {
                    statusCode = context.Response.StatusCode,
                    mensaje = "Ocurrió un error interno en el servidor."
                };

                var json = JsonSerializer.Serialize(respuesta);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
