public class Program
{
    public static void Main()
    {
        // ���÷�����IP�Ͷ˿�
        GameServe.Instance.Init("0.0.0.0", 8888); // 0.0.0.0��ʾ��������IP
        GameServe.Instance.Start();

        // �������߳�����
        Console.WriteLine("Press any key to stop the server...");
        Console.ReadKey();
        GameServe.Instance.Stop();
    }
}