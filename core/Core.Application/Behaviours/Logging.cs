using System.Diagnostics;
using Core.Application.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Application.Behaviours;

public class LoggingBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest : global::MediatR.IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        
        var unqiueId = Guid.NewGuid().ToString();
        
        var requestJson = SerializerHelper.JsonSerialize(request);
        
        _logger.LogInformation($"Begin Request Id:{unqiueId}, request name:{requestName}, request json:{requestJson}");
        
        var timer = new Stopwatch();
        
        timer.Start();
        
        var response = await next();
        
        timer.Stop();

        var resposeJson = SerializerHelper.JsonSerialize(response);
        
        _logger.LogInformation($"End Request Id:{unqiueId}, request name:{requestName}, total request time:{timer.ElapsedMilliseconds}ms, response json: {resposeJson}");
        
        return response;
    }
}