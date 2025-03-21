using ShareProtobuf;

public class HeartbeatRequest : MessageRquestBase
{
   
    public override async Task ReadFromStream(byte[] messageBuffer)
    {
        // 读取数据
        Heartbeat heartbeat = await GetClientHandle().ReceiveMessage<Heartbeat>(messageBuffer);
        // 处理数据
        Console.WriteLine("HeartbeatRequest Heartbeat: {0}", heartbeat.Timestamp);
        // 返回数据
        HeartbeatAck heartbeatResponse = new HeartbeatAck
        {
            Timestamp = heartbeat.Timestamp,
        };
        this.GetClientHandle().ConnectDataTime = DateTime.Now;
        await GetClientHandle().SendMessage(MessageRequestType.HeartbeatAck, heartbeatResponse);
        Console.WriteLine("HeartbeatRequest HeartbeatAck");
    }
}