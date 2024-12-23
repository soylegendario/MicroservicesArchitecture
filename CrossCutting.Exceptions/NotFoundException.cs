using System.Runtime.Serialization;

namespace CrossCutting.Exceptions;

public abstract class NotFoundException : ApplicationException
{
    protected NotFoundException()
    {
        
    }

    protected NotFoundException(string message) : base(message)
    {
        
    }

    protected NotFoundException(string message, Exception inner) : base(message, inner)
    {
        
    }

    [Obsolete("Obsolete")]
    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        
    }
}