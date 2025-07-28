using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SuperMarketApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log the exception with all inner exception details
            LogExceptionWithInnerExceptions(exception);

            // Determine appropriate HTTP status code based on exception type
            var (statusCode, message) = GetStatusCodeAndMessage(exception);
            
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            // Create a generic error response for users
            var errorResponse = new
            {
                Message = message,
                StatusCode = context.Response.StatusCode
            };

            // Serialize and send the response
            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private (int statusCode, string message) GetStatusCodeAndMessage(Exception exception)
        {
            return exception switch
            {
                BadHttpRequestException => ((int)HttpStatusCode.BadRequest, "Invalid request. Please check your input and try again."),
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "You are not authorized to perform this action."),
                //ArgumentException => ((int)HttpStatusCode.BadRequest, "Invalid parameters provided."),
                ArgumentNullException => ((int)HttpStatusCode.BadRequest, "Required parameters are missing."),
                InvalidOperationException => ((int)HttpStatusCode.BadRequest, "The requested operation cannot be performed."),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "The requested resource was not found."),
                _ => ((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request. Please try again later.")
            };
        }

        private void LogExceptionWithInnerExceptions(Exception exception)
        {
            var exceptionDetails = new System.Text.StringBuilder();
            exceptionDetails.AppendLine($"Exception occurred: {exception.Message}");
            exceptionDetails.AppendLine($"Exception Type: {exception.GetType().Name}");
            exceptionDetails.AppendLine($"Stack Trace: {exception.StackTrace}");

            // Log inner exceptions recursively
            var innerException = exception.InnerException;
            var innerLevel = 1;
            while (innerException != null)
            {
                exceptionDetails.AppendLine($"Inner Exception Level {innerLevel}:");
                exceptionDetails.AppendLine($"  Message: {innerException.Message}");
                exceptionDetails.AppendLine($"  Type: {innerException.GetType().Name}");
                exceptionDetails.AppendLine($"  Stack Trace: {innerException.StackTrace}");
                exceptionDetails.AppendLine();

                innerException = innerException.InnerException;
                innerLevel++;
            }

            // Log with appropriate level based on exception type
            if (exception is BadHttpRequestException || exception is ArgumentException || exception is ArgumentNullException)
            {
                _logger.LogWarning(exception, "Client Error Exception: {ExceptionDetails}", exceptionDetails.ToString());
            }
            else if (exception is UnauthorizedAccessException)
            {
                _logger.LogWarning(exception, "Authorization Exception: {ExceptionDetails}", exceptionDetails.ToString());
            }
            else if (exception is KeyNotFoundException)
            {
                _logger.LogInformation(exception, "Resource Not Found: {ExceptionDetails}", exceptionDetails.ToString());
            }
            else
            {
                _logger.LogError(exception, "Unhandled Exception: {ExceptionDetails}", exceptionDetails.ToString());
            }
        }
    }
} 