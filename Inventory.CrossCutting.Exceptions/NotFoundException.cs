using System.Runtime.Serialization;

namespace Inventory.CrossCutting.Exceptions;

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

    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        
    }
}