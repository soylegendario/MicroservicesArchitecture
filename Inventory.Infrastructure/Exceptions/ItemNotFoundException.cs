using System.Runtime.Serialization;

namespace Inventory.Infrastructure.Exceptions;

[Serializable]
public sealed class ItemNotFoundException : Exception    
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

    private ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        
    }
}
