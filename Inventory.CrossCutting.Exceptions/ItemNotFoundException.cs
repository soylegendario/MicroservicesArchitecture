using System.Runtime.Serialization;

namespace Inventory.CrossCutting.Exceptions;

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

    private ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        
    }
}
