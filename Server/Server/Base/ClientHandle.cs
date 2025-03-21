using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using ProtoBuf;
using ShareProtobuf;
 
public class ClientHandle:IDisposable
{
    private readonly TcpClient _client;
    private readonly NetworkStream _stream;

    //监听客户端 断开
    public event EventHandler OnClientClose;
    public DateTime ConnectDataTime { get; set; }
    public string ClientRemoteEndPoint { get; set; }


    public ClientHandle(TcpClient client)
    {
        _client = client;
        _stream = _client.GetStream();
        ConnectDataTime = DateTime.Now;
        ClientRemoteEndPoint = _client.Client.RemoteEndPoint.ToString();
    }

    public async Task CheckClientInactiveAsync()
    { 
        while (_client.Connected)
        {
            if (DateTime.Now - ConnectDataTime > TimeSpan.FromSeconds(10))
            {
                Console.WriteLine("客户端超时断开");
                Close();
                OnClientClose?.Invoke(this, EventArgs.Empty);
                break;
            }
            await Task.Delay(1000);
        }
        Console.WriteLine("CheckClientInactiveAsync end");
        Close();
        OnClientClose?.Invoke(this, EventArgs.Empty);
    }

    //处理客户端请求
    public async Task HandleClientAsync()
    {
        try 
        {
            byte[] typeBuffer = new byte[1];
            byte[] lengthBuffer = new byte[4];
            byte[] messageBuffer;
            while (_client.Connected)
            {
                // 1. 读取类型（1字节）
                await _stream.ReadAsync(typeBuffer, 0, 1);
                MessageRequestType type = (MessageRequestType)typeBuffer[0];

                // 2. 读取长度（4字节，大端序转int）
                await _stream.ReadAsync(lengthBuffer, 0, 4);
                int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(lengthBuffer, 0));

                // 3. 读取消息体
                messageBuffer = new byte[length];
                await _stream.ReadAsync(messageBuffer, 0, length);

                if (type > MessageRequestType.None)
                { 
                    var request = ServerFactory.Instance.GetMessageRquestFactory().GetObject(type);
                    if (request != null)
                    {
                        request.SetClientHandle(this);
                        await request.ReadFromStream(messageBuffer);
                    }
                    ServerFactory.Instance.GetMessageRquestFactory().PutObject(request);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("HandleClientAsync error: {0}", ex.Message);
        }
        finally
        {
            this.Close();
            OnClientClose?.Invoke(this, EventArgs.Empty);
        }
    }

    
    public void Close()
    {
        _client.Close();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _client?.Dispose();
            _stream?.Dispose();
        }
    }

    public async Task<T> ReceiveMessage<T>(byte[] messageBuffer)
    {
        using var memoryStream = new MemoryStream(messageBuffer);
        return Serializer.Deserialize<T>(memoryStream);
    }

    // 统一发送消息方法
    public async Task SendMessage(MessageRequestType type, object message)
    {
        if (_stream == null || !_stream.CanWrite)
        {
            Console.WriteLine("流不可用，无法发送消息");
            return;
        }

        try
        {
            // 将消息类型和消息体合并为一个完整包
            using (var memoryStream = new MemoryStream())
            {
                // 1. 写入1字节类型标识
                memoryStream.WriteByte((byte)type);

                // 2. 序列化消息体到独立内存流（获取二进制数据）
                byte[] messageBytes;
                using (var msgStream = new MemoryStream())
                {
                    Serializer.Serialize(msgStream, message);
                    messageBytes = msgStream.ToArray();
                }

                // 3. 写入消息体长度（4字节，大端序）
                byte[] lengthBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(messageBytes.Length));
                memoryStream.Write(lengthBytes, 0, 4);

                // 4. 写入消息体
                memoryStream.Write(messageBytes, 0, messageBytes.Length);

                // 5. 异步发送整个包
                byte[] packet = memoryStream.ToArray();
                await _stream.WriteAsync(packet, 0, packet.Length);
                await _stream.FlushAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("SendMessage error: {0}", ex.Message);
        }
       
        await _stream.FlushAsync();
    }

}