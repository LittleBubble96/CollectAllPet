public abstract class BTCfgBase
{
    protected abstract ConfBTCShape CreateConfBtcShape();
    protected ConfBTCShape root;
    public ConfBTCShape Root 
    {
        get
        {
            if (root == null)
            {
                root = CreateConfBtcShape();
            }
            return root;
        }
    }
}