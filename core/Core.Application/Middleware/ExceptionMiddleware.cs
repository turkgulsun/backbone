
using System.Net;
using Core.Application.Common;
using Core.Application.Exceptions;
using Core.Application.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.Application.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception? exceptionObj)
        {
            await HandleExceptionAsync(context, exceptionObj, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception? exception, ILogger<ExceptionMiddleware> logger)
    {
        int code;

        var error = GetErrorFromException(exception);

        var result = SerializerHelper.JsonSerialize(error);

        switch (exception)
        {
            case BadRequestException badRequestException:
                code = (int)HttpStatusCode.BadRequest;
                break;
            case DeleteFailureException deleteFailureException:
                code = (int)HttpStatusCode.BadRequest;
                break;
            case InvalidOperationException invalidOperationException:
                code = (int)HttpStatusCode.InternalServerError;
                break;
            case NotFoundException _:
                code = (int)HttpStatusCode.NotFound;
                break;
            default:
                code = (int)HttpStatusCode.InternalServerError;
                break;
        }

        logger.LogError(result);

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = code;

        var baseResult = BaseResult<Error>.Fail(code.ToString(), "Exception Middleware", error);

        var response =
            SerializerHelper.JsonSerialize(baseResult);

        return context.Response.WriteAsync(response);
    }

    private static Error GetErrorFromException(Exception? exception)
    {
        var error = new Error();
        while (exception != null)
        {
            error.ErrorInfos.Add(new ErrorInfo { Message = exception.Message, StackTrace = exception.StackTrace, Source = exception.Source });

            exception = exception.InnerException;
        }

        return error;
    }
}