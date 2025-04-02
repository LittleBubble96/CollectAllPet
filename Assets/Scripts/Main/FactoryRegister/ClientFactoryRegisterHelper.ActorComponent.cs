public partial class ClientFactoryRegisterHelper
{
    public static void RegisterActorComponent()
    {
        ClientFactory.Instance.GetActorComponentFactory().RegisterType<TargetComponent>(typeof(TargetComponent));
    }
}