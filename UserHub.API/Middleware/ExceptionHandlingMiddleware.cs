using System;
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
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError("An unhandled exception occurred while executing {Method} {Path}", context.Request.Method, context.Request.Path);


            if (context.Response.HasStarted)
            {
                throw;
            }

            var ProblemDetails = new ProblemDetails
            {
                Title = "An unexpected error occured while processing your request.",
                Type = "application/problem+json",
                Status = StatusCodes.Status500InternalServerError,
                Instance = "traceId"
            };
        }
    }
}