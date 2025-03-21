public class ServerFactory
{
    private readonly MutilFactoryWithPool<MessageRquestBase> MessageRquestFactory  = new MutilFactoryWithPool<MessageRquestBase>();

    protected static ServerFactory _instance = new ServerFactory();

    public MutilFactoryWithPool<MessageRquestBase> GetMessageRquestFactory()
    {
        return MessageRquestFactory;
    }

    public static ServerFactory Instance
    {
        get
        {
            return _instance;
        }
    }

}