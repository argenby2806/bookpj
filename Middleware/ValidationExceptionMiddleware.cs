using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace bookpj.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException vex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = vex.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage })
                    .ToList();

                var payload = JsonSerializer.Serialize(new
                {
                    Message = "Validation failed",
                    Errors = errors
                });

                await context.Response.WriteAsync(payload);
            }
        }
    }
}
