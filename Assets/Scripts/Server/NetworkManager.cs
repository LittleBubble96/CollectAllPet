using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProtoBuf;
using ShareProtobuf;
using UnityEngine;

public class NetworkManager 
{
    private TcpClient _client;
    private NetworkStream _stream;

    private DateTime _lastReceivedTime;
    
    //缓存当前激活的response
    private ConcurrentDictionary<Enum,ConcurrentQueue<ClientMessageRequestBase>> _responseCache = new ConcurrentDictionary<Enum, ConcurrentQueue<ClientMessageRequestBase>>();
    public void Init()
    {
        this.ConnectToMasterServer("127.0.0.1", 8888);
        Application.quitting += () =>
        {
            Disconnect();
        };
    }
    

    async void ConnectToMasterServer(string ip, int port)
    {
        try
        {
            _client = new TcpClient();
            await _client.ConnectAsync(ip, port);
            _stream = _client.GetStream();
            _lastReceivedTime = DateTime.Now;
            Debug.Log("Connected to master server!");
            _ = AcceptClientAsync();
            _ = SendHeartbeatAsync();
            _ = CheckOfflineAsync();
        }
        catch (Exception e)
        {
            Debug.Log("Failed to connect to master server: " + e.Message);
            throw;
        }
    }
    
    // 发送心跳包
    private async Task SendHeartbeatAsync()
    {
        while (_client.Connected)
        {
            try
            {
                await Task.Delay(1000);
                Heartbeat heartbeat = new Heartbeat();
                heartbeat.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                Debug.Log("Send heartbeat to master server!");
                await SendRequest(MessageRequestType.Heratbeat, heartbeat);
            }
            catch (Exception e)
            {
                Debug.Log("Failed to send heartbeat to master server: " + e.Message);
                Disconnect();
                throw;
            }
        }
    }
    
    //循环检测是否离线
    private async Task CheckOfflineAsync()
    {
        while (_client.Connected)
        {
            await Task.Delay(1000);
            if ((DateTime.Now - _lastReceivedTime).TotalSeconds > 10)
            {
                Debug.Log("Master server is offline!");
                // Disconnect();
                break;
            }
        }
    }

    private async Task AcceptClientAsync()
    {
        byte[] typeBuffer = new byte[1];
        byte[] lengthBuffer = new byte[4];
        byte[] messageBuffer;
        while (_client.Connected)
        {
            // 1. 读取消息类型（1字节）
            Debug.Log("[AcceptClientAsync] Read message type");
            int byteRead = await _stream.ReadAsync(typeBuffer,0,1);
            if (byteRead == 0)
            {
                Debug.Log("[AcceptClientAsync] Read 0 bytes, master server is offline!");
                break;
            }
               
            MessageRequestType type = (MessageRequestType)typeBuffer[0];
            Debug.Log("Received message type: " + type);
            // 2. 读取长度（4字节，大端序转int）
            await _stream.ReadAsync(lengthBuffer, 0, 4);
            int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(lengthBuffer, 0));
            Debug.Log("Received message length: " + length);
            // 3. 读取消息体
            messageBuffer = new byte[length];
            await _stream.ReadAsync(messageBuffer, 0, length);
            using var memoryStream = new MemoryStream(messageBuffer);
            if (type <= MessageRequestType.None)
            {
                continue;
            }
            
            // 查看是否有对应的response
            var response = GetResponseCache(type);
            if (response != null)
            {
                Debug.Log("Handle response: " + type);
                await response.HandleResponse(type, messageBuffer);
            }
            Debug.Log("last Received message: " + type);
            // try
            // { 
            //     
            // }
            // catch (Exception e)
            // {
            //     Debug.Log("Failed to read data from master server: " + e.Message);
            //     Disconnect();
            //     break;
            // }
        }
        Debug.Log("[AcceptClientAsync] client is offline!");
    }
    
    //添加response到缓存
    public void AddResponseCache(ClientMessageRequestBase request)
    {
        var key = request.GetResponseMessageType();
        if (!_responseCache.ContainsKey(key))
        {
            _responseCache.TryAdd(key, new ConcurrentQueue<ClientMessageRequestBase>());
        }

        if ((MessageRequestType)key > MessageRequestType.None)
        {
            _responseCache[key].Enqueue(request);
        }
    }
    
    //从缓存中获取response
    public ClientMessageRequestBase GetResponseCache(Enum key)
    {
        if (_responseCache.ContainsKey(key))
        {
            if (_responseCache[key].TryDequeue(out var response))
            {
                return response;
            }
        }
        return null;
    }
    
    //发送request
    public async Task SendRequest(MessageRequestType type , object msg)
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
                    Serializer.Serialize(msgStream, msg);
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
       
        //response 加入缓存
        var response = ClientFactory.Instance.GetMessageResponseFactory().GetObject(type);
        if (response != null)
        {
            AddResponseCache(response);
        }
    }
    
    
    public async Task<T> ReceiveMessage<T>(byte[] messageBuffer)
    {
        using var memoryStream = new MemoryStream(messageBuffer);
        return Serializer.Deserialize<T>(memoryStream);
    }
    
    //重置最后接收时间
    public void ResetLastReceivedTime()
    {
        _lastReceivedTime = DateTime.Now;
    }
    
    public void Disconnect()
    {
        Debug.Log("Disconnected from master server!");
        _client.Dispose();
        _client.Close();
        _stream.Close();
    }
}