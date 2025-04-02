using UnityEngine;

public class BehaviorTree
{
    private BlackBoard blackBoard;
    
    private BehaviorNode rootNode;
    
    private BTGenInfo btGenInfo;
    
    private BtNodeResult btRootRunRes;
    
    private AIController aiController;
    
    public void Init(BTGenInfo info , AIController ai)
    {
        this.btGenInfo = info;
        this.aiController = ai;
        this.blackBoard = new BlackBoard();
        this.rootNode = InitTree();
    }
    
    public void Recycle()
    {
        this.blackBoard.Clear();
        this.blackBoard = null;
        if (this.rootNode != null)
        {
            this.rootNode.Recycle();
            ClientFactory.Instance.GetBehaviorNodeFactory().PutObject(this.rootNode);
            this.rootNode = null;
        }
        this.btGenInfo = null;
        this.btRootRunRes = BtNodeResult.None;
    }

    public BlackBoard GetBlackBoard()
    {
        return this.blackBoard;
    }

    public BTGenInfo GetBTGenInfo()
    {
        return this.btGenInfo;
    }
    
    public AIController GetAIController()
    {
        return this.aiController;
    }
    
    private BehaviorNode InitTree()
    {
        ConfBTCShape cfgBtData = BTConfigHelper.Instance.GetConfBtcShape(btGenInfo.GetBtGenId());
        if (cfgBtData == null)
        {
            Debug.LogError($"BehaviorTree InitTree: cfgBTData is null, btGenId: {btGenInfo.GetBtGenId()}");
            return null;
        }

        BehaviorNode root = ClientFactory.Instance.GetBehaviorNodeFactory().GetObject(cfgBtData.BTNodeType);
        root.AssembleBTNode(this, cfgBtData);
        return root;
    }


    public void Execute(float deltaTime)
    {
        if (this.rootNode == null)
        {
            return;
        }

        if (btRootRunRes != BtNodeResult.InProgress)
        {
            rootNode.Begin();
        }
        rootNode.Execute(deltaTime);
        btRootRunRes = rootNode.RunState();
        switch (btRootRunRes)
        {
            case BtNodeResult.Succeeded:
                case BtNodeResult.Failed:
                rootNode.End();
                break;
            case BtNodeResult.InProgress:
                break;
            default:
                this.btRootRunRes = BtNodeResult.Failed;
                break;
        }
    }

}

/**
 * 行为树自定义数据
 */
public enum BtNodeResult
{
    None = 0,
    Succeeded = 1,
    Failed,
    InProgress,
}