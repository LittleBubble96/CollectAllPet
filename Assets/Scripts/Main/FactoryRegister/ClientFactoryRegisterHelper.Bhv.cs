public partial class ClientFactoryRegisterHelper
{
    public static void RegisterGameBhv()
    {
        ClientFactory.Instance.GetBehaviorNodeFactory().RegisterType<BTSelectorNode>(typeof(BTSelectorNode));
        ClientFactory.Instance.GetBehaviorNodeFactory().RegisterType<BTSequenceNode>(typeof(BTSequenceNode));
        ClientFactory.Instance.GetBehaviorNodeFactory().RegisterType<BTPetAIBreakDN>(typeof(BTPetAIBreakDN));
        ClientFactory.Instance.GetBehaviorNodeFactory().RegisterType<BTPetHatedTN>(typeof(BTPetHatedTN));
        ClientFactory.Instance.GetBehaviorNodeFactory().RegisterType<BTPetPreBhvTN>(typeof(BTPetPreBhvTN));
        ClientFactory.Instance.GetBehaviorNodeFactory().RegisterType<BTPetAttackTN>(typeof(BTPetAttackTN));
    }
}