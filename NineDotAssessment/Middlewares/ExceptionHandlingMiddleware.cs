using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace NineDotAssessment.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception.
            Log.Error(ex, "An unhandled exception occurred.");

            // Handle the exception and return an appropriate response to the client.
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new
        {
            message = "An error occurred while processing your request.",
            exceptionMessage = exception.Message,
            //stackTrace = exception.StackTrace
        };

        return context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
    }
}
