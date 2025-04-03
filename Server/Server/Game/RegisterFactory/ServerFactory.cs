public partial class ServerFactory
{
    private readonly MutilFactoryWithPool<MessageRquestBase> MessageRquestFactory  = new MutilFactoryWithPool<MessageRquestBase>();
    private readonly FactoryWithPool<GameRoomEffectData> GameRoomEffectDataFactory = new FactoryWithPool<GameRoomEffectData>();

    protected static ServerFactory _instance = new ServerFactory();

    public MutilFactoryWithPool<MessageRquestBase> GetMessageRequestFactory()
    {
        return MessageRquestFactory;
    }
    
    public FactoryWithPool<GameRoomEffectData> GetGameRoomEffectDataFactory()
    {
        return GameRoomEffectDataFactory;
    }

    public static ServerFactory Instance
    {
        get
        {
            return _instance;
        }
    }

}