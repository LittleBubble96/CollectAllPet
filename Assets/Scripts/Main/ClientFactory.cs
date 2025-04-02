public class ClientFactory
{
    private readonly MutilFactoryWithPool<ClientMessageRequestBase> messageResponseFactory  = new MutilFactoryWithPool<ClientMessageRequestBase>();
    private readonly MutilFactoryWithPool<MessageRequestBase> messageRquestFactory  = new MutilFactoryWithPool<MessageRequestBase>();
    private readonly MutilFactoryWithPool<GameStateBase> gameStateFactory = new MutilFactoryWithPool<GameStateBase>();
    private readonly MutilTypeFactoryWithPool<BehaviorNode> behaviorNodeFactory = new MutilTypeFactoryWithPool<BehaviorNode>();
    private readonly MutilTypeFactoryWithPool<ActorComponent> actorComponentFactory = new MutilTypeFactoryWithPool<ActorComponent>();
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
    
    public MutilTypeFactoryWithPool<BehaviorNode> GetBehaviorNodeFactory()
    {
        return behaviorNodeFactory;
    }

    public MutilTypeFactoryWithPool<ActorComponent> GetActorComponentFactory()
    {
        return actorComponentFactory;
    }

    public static ClientFactory Instance
    {
        get
        {
            return _instance;
        }
    }

}