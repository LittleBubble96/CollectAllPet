public class ClientFactory
{
    private readonly MutilFactoryWithPool<ClientMessageRequestBase> messageResponseFactory  = new MutilFactoryWithPool<ClientMessageRequestBase>();
    private readonly MutilFactoryWithPool<MessageRequestBase> messageRquestFactory  = new MutilFactoryWithPool<MessageRequestBase>();
    private readonly MutilFactoryWithPool<GameStateBase> gameStateFactory = new MutilFactoryWithPool<GameStateBase>();

    protected static ClientFactory _instance = new ClientFactory();

    public MutilFactoryWithPool<ClientMessageRequestBase> GetMessageResponseFactory()
    {
        return messageResponseFactory;
    }
    
    public MutilFactoryWithPool<MessageRequestBase> GetMessageRequestFactory()
    {
        return messageRquestFactory;
    }
    
    public MutilFactoryWithPool<GameStateBase> GetGameStateFactory()
    {
        return gameStateFactory;
    }

    public static ClientFactory Instance
    {
        get
        {
            return _instance;
        }
    }

}