using System;

public class BT_1001 : BTCfgBase
{
    protected override ConfBTCShape CreateConfBtcShape()
    {
        return new ConfBTCShape()
        {
            Description = "选择",
            BTNodeType = typeof(BTSelectorNode),
            Children = new[]
            {
                new ConfBTCShape()
                {
                    Description = "顺序",
                    BTNodeType = typeof(BTSequenceNode),
                    Decorators = new[]
                    {
                        new ConfBTCShape()
                        {
                            Description = "中断判定",
                            BTNodeType = typeof(BTPetAIBreakDN),
                        }
                    },
                    Services = Array.Empty<ConfBTCShape>(),
                    Children = new[]
                    {
                        new ConfBTCShape()
                        {
                            Description = "目标选择",
                            BTNodeType = typeof(BTPetHatedTN),
                        },
                        new ConfBTCShape()
                        {
                            Description = "预备行为",
                            BTNodeType = typeof(BTPetPreBhvTN),
                        },
                        new ConfBTCShape()
                        {
                            Description = "攻击行为",
                            BTNodeType = typeof(BTPetAttackTN),
                        }
                    }
                }
            }

        };
    }
}