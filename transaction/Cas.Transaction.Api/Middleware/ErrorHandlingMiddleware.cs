using Cas.Transaction.Core.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cas.Transaction.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, this.logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {
            object errors = null;

            switch (ex)
            {
                case NotFoundException re:
                    errors = re.Errors;
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case Exception:
                    logger.LogError(ex, "Server Error");
                    errors = string.IsNullOrWhiteSpace(ex.Message) ? "Error" : ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            if (errors != null)
            {
                var result = JsonSerializer.Serialize(new { errors });
                await context.Response.WriteAsync(result);
            }
        }
    }
}
