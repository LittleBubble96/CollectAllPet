public class BehaviorTree
{
    private BlackBoard blackBoard;
    
    private BehaviorNode rootNode;
    
    private BTGenInfo btGenInfo;
    
    public void Init(BTGenInfo info)
    {
        this.btGenInfo = info;
        this.blackBoard = new BlackBoard();
        this.rootNode = InitTree();
    }
    
    private BehaviorNode InitTree()
    {
        return null;
    }
    
    
}