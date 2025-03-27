

using System.Threading.Tasks;

public class MessageRequestBase : IRecycle
{

    //处理消息
    public virtual async Task ReadFromStream(byte[] messageBuffer)
    {

    }

    public void Recycle()
    { 
    
    }

    
}