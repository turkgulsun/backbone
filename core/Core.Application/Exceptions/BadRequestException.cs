using System.Runtime.Serialization;

namespace Core.Application.Exceptions;

public class BadRequestException: Exception
{
    public BadRequestException(string message)
        : base(message)
    {
    }

    protected BadRequestException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}