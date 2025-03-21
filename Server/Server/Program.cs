public class Program
{
    public static void Main()
    {
        // 配置服务器IP和端口
        GameServe.Instance.Init("0.0.0.0", 8888); // 0.0.0.0表示监听所有IP
        GameServe.Instance.Start();

        // 保持主线程运行
        Console.WriteLine("Press any key to stop the server...");
        Console.ReadKey();
        GameServe.Instance.Stop();
    }
}