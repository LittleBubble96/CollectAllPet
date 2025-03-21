public partial class ClientFactoryRegisterHelper
{
    public static void RegisterGameState()
    {
        ClientFactory.Instance.GetGameStateFactory().RegisterType<LoginGameState>(GameStateEnum.Login);
        ClientFactory.Instance.GetGameStateFactory().RegisterType<MainGameState>(GameStateEnum.Main);
    }
}