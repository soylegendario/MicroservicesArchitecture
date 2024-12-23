using System.Runtime.Serialization;
using CrossCutting.Exceptions;

namespace Inventory.Application.Exceptions;

[Serializable]
public sealed class ItemNotFoundException : NotFoundException    
{
    public ItemNotFoundException()
    {
        
    }

    public ItemNotFoundException(string message) : base(message)
    {
        
    }

    public ItemNotFoundException(string message, Exception inner) : base(message, inner)
    {
        
    }

    [Obsolete("Obsolete")]
    private ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        
    }
}
