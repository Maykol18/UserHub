using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace UserHub.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //Continue executing the  HTTP request pipeline.
            await _next(context);
        }
        catch (Exception exception)
        {
            //Log the exception on the server, including its stack trace.
            _logger.LogError(exception, "An unhandled exception occurred while executing {Method} {Path}", context.Request.Method, context.Request.Path);

            //If the response has already started, we cannot replace it.
            if (context.Response.HasStarted)
            {
                throw;
            }

            //Discard any response content written before the exception.
            context.Response.Clear();

            //Create a safe, standarized error response.
            var ProblemDetails = new ProblemDetails
            {
                Title = "An unexpected error occured.",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An unexpected error occurred while proccessing your request.",
                Instance = context.Request.Path
            };

            //Include a trace ID for troubleshooting.
            ProblemDetails.Extensions["traceId"] = context.TraceIdentifier;

            //Configure the HTTP response.
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            //Write ProblemDetails as JSON.
            await context.Response.WriteAsJsonAsync(
                ProblemDetails,
                options: null,
                contentType: "application/problem+json"
                );
        }
    }
}