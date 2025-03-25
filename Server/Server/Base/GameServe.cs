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
    
    //开始时间
    private Thread updateThread;

    public void Init(string ip, int port)
    {
        this.iPAddress = IPAddress.Parse(ip);
        this.port = port;

        //开启更新线程
        updateThread = new Thread(Update)
        {
            IsBackground = true, // 设为后台线程（主线程退出时自动终止）
            Priority = ThreadPriority.AboveNormal // 提高线程优先级
        };
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

    //update
    private void Update()
    {
        double lag = 0;
        long startTime = TimeHelper.GetTimeStamp();
        while (isRunning)
        {
            long currentTime = TimeHelper.GetTimeStamp();
            long elapsedTime = currentTime - startTime;
            startTime = currentTime;
            lag += elapsedTime;
            while (lag >= GameConst.FrameInterval)
            {
                GameRoomManager.Instance.Update();
                lag -= GameConst.FrameInterval;
            }
            Thread.Sleep(1);
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

    public ClientHandle GetClientHandle(string clientId)
    {
        if (_clients.TryGetValue(clientId, out var client))
        {
            return client;
        }

        return null;
    }

}