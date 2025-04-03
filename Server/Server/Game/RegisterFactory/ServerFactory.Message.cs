
using ShareProtobuf;

public partial class ServerFactory
{
    private readonly FactoryWithPool<SyncPlayEffectToClientRequest> playEffectDataFactory = new FactoryWithPool<SyncPlayEffectToClientRequest>();
    private readonly FactoryWithPool<SyncPlayEffectToClientData> playEffectData = new FactoryWithPool<SyncPlayEffectToClientData>();
    
    public FactoryWithPool<SyncPlayEffectToClientRequest> GetPlayEffectDataFactory()
    {
        return playEffectDataFactory;
    }
    
    public FactoryWithPool<SyncPlayEffectToClientData> GetPlayEffectData()
    {
        return playEffectData;
    }
}