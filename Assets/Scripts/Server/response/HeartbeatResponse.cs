using System.Net.Sockets;
using System.Threading.Tasks;
using ShareProtobuf;

public class HeartbeatResponse : ClientMessageRequestBase
{
    public override async Task HandleResponse(MessageRequestType msgResponseType, byte[] messageBuffer)
    {
        HeartbeatAck heartbeatResponse = await GameManager.GetNetworkManager().ReceiveMessage<HeartbeatAck>(messageBuffer);
        GameManager.GetNetworkManager().ResetLastReceivedTime();
    }

    public override MessageRequestType GetRequestMessageType()
    {
        return MessageRequestType.Heratbeat;
    }

    public override MessageRequestType GetResponseMessageType()
    {
        return MessageRequestType.HeartbeatAck;
    }
}