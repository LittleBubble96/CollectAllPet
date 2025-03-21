using ProtoBuf;
using System.IO;
using System.Net.Sockets;

public class MessageRquestBase : IRecycle
{

    private ClientHandle clientHandle;

    public void SetClientHandle(ClientHandle inClientHandle)
    {
        clientHandle = inClientHandle;
    }

    public ClientHandle GetClientHandle()
    {
        return clientHandle;
    }
    //处理消息
    public virtual async Task ReadFromStream(byte[] messageBuffer)
    {

    }

    public void Recycle()
    { 
    
    }

    
}