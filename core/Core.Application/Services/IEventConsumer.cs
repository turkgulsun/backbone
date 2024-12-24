namespace Core.Application.Services;

/// <summary>
/// Event Consumer
/// </summary>
public interface IEventConsumer
{
    Task ConsumeEvent(string topic, Func<string, Task> callback, CancellationToken cancellationToken);
}
