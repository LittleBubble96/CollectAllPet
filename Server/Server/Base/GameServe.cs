using ShareProtobuf;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class GameServe : Singleton<GameServe>
{
    private TcpListener _listener; // TCp监听器
    private bool isRunning = false; // 是否正在运行

    //服务器配置参数
    private IPAddress iPAddress;
    private  int port;

    //客户端
    private ConcurrentDictionary<string,ClientHandle> _clients = new ConcurrentDictionary<string,ClientHandle>();
    

    public void Init(string ip, int port)
    {
        this.iPAddress = IPAddress.Parse(ip);
        this.port = port;
    }

    public void Start()
    {
        try 
        { 
            _listener = new TcpListener(iPAddress, port);
            _listener.Start();
            isRunning = true;
            Console.WriteLine("服务器启动成功 {0}:{1}", iPAddress, port);
            //注册消息工厂
            RequestMessageFactoryHelper.Register();
            GameRoomManager.Instance.Init();
            PlayerManager.Instance.Init();
            Task.Run(() => AcceptClientAsync());
        }
        catch (Exception ex)
        {
            Console.WriteLine("服务器启动失败: {0}", ex.Message);
        }
    }

    public void Stop()
    {
        isRunning = false;
        _listener.Stop();
        foreach (var client in _clients)
        {
            client.Value.Close();
        }
        _clients = new ConcurrentDictionary<string, ClientHandle>();
        Console.WriteLine("服务器停止成功");
    }

    private async Task AcceptClientAsync()
    { 
        while ( isRunning)
        {
            try 
            {
            
                var client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine("客户端连接 {0}", client.Client.RemoteEndPoint);

                // 创建客户端处理实例（不包裹在using中）
                var clientHandle = new ClientHandle(client);

                // 管理客户端集合（需线程安全）
                _clients.TryAdd(clientHandle.ClientRemoteEndPoint, clientHandle);

                // 注册断开事件
                clientHandle.OnClientClose += (sender, e) =>
                {
                    _clients.TryRemove(clientHandle.ClientRemoteEndPoint, out _);
                    clientHandle.Dispose(); // 显式释放资源
                    Console.WriteLine("客户端断开连接 {0}", clientHandle.ClientRemoteEndPoint);
                };

                // 启动独立任务处理客户端（不阻塞主循环）
                _ = Task.Run(async () =>
                {
                    try
                    {
                        _ = clientHandle.CheckClientInactiveAsync();
                        _ = clientHandle.HandleClientAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"客户端处理异常: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("AcceptClientAsync error: {0}", ex.Message);
            }
           
        }
    }

}