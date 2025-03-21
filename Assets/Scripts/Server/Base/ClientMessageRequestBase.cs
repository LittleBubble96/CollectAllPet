
using System.Net.Sockets;
using System.Threading.Tasks;
using ShareProtobuf;

public class ClientMessageRequestBase : IRecycle
{
    
    public virtual async Task HandleResponse(MessageRequestType msgResponseType , byte[] messageBuffer)
    {
        
    }
    
    public virtual void HandleRequest(NetworkStream stream)
    {
        
    }
    
    public virtual MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.None;
    }
    
    public virtual MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.None;
    }
    
    public void Recycle()
    {
        
    }
}