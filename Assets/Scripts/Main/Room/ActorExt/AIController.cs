using System.Collections.Generic;
using ShareProtobuf.ShareData;
using UnityEngine;
using UnityEngine.AI;

public class AIController : Actor
{
    //行为树
    protected BehaviorTree behaviorTree;
    // 输入历史队列（用于回溯）
    private Queue<PlayerInput> _inputQueue = new Queue<PlayerInput>();
    //Nav
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField] private float attackAnimDuration = 0.875f;
    protected override void OnInit()
    {
        base.OnInit();
        behaviorTree = new BehaviorTree();
        behaviorTree.Init(new BTGenInfo(GetAiId()),this);
        //停止距离 为 攻击距离
        navMeshAgent.stoppingDistance = GetAttackDistance();
    }

    protected override void DirectUpdate()
    {
        base.DirectUpdate();
        if (behaviorTree != null)
        {
            behaviorTree.Execute(Time.fixedDeltaTime);
        }
        //
        float dt = Time.fixedDeltaTime;
        if (GetActorState() == EActorState.Syncing)
        {
            _inputQueue.Enqueue(new PlayerInput
            {
                deltaTime = dt,
                MoveDirection = navMeshAgent.velocity,
            });
        }
        // lerp 旋转
        Vector3 targetDir = navMeshAgent.desiredVelocity;
        if (targetDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, dt * 10f);
        }
        float speed = navMeshAgent.velocity.magnitude;
        GetAnimationController().SetFloat("MoveSpeed",speed);
            
        SetSpeed(transform.position - GetPosition() / dt);
        SetPosition(transform.position);
        SetRotation(transform.eulerAngles);
    }
    
    public bool IsAgentStopped()
    {
        float velocity = navMeshAgent.velocity.magnitude;
        Debug.Log($"[AI] IsAgentStopped: {velocity}");
        return navMeshAgent != null  &&  
               navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
               navMeshAgent.velocity.magnitude < 0.1f;
    }

    #region sync 同步

    protected override void OnChangeState(EActorState state)
    {
        base.OnChangeState(state);
        if (state == EActorState.Syncing)
        {
            _inputQueue.Clear();
        }
    }

    public override void SetServerPosition(Vector3 position)
    {
        base.SetServerPosition(position);
        // if (IsHost())
        // {
        //     //做一个误差校正
        //     Vector3 tfPos = transform.position;
        //     // transform.position = position;
        //     Vector3 serverPos = GetServerPosition();
        //     navMeshAgent.Move(serverPos - tfPos);
        //     int count = _inputQueue.Count;
        //     while (_inputQueue.Count > 0)
        //     {
        //         PlayerInput input = _inputQueue.Dequeue();
        //         navMeshAgent.Move(input.MoveDirection * input.deltaTime);
        //     }
        //     Debug.Log( $"[AI]SetServerPosition: {tfPos} -> {serverPos} count: {count}");
        // }
    }

    public override void SetServerRotation(Vector3 rotation)
    {
        if (IsHost())
        {
            serverRotation = rotation;
        }
        else
        {
            base.SetServerRotation(rotation);
        }
    }

    #endregion


    protected int GetAiId()
    {
        //TODO 目前就是只有宠物用按这个脚本所以获取宠物配置
        MonsterConfigItem cfg = MonsterConfig.GetConfigItem(this.actorInfo.ActorConfigId);
        if (cfg == null)
        {
            return -1;
        }
        return cfg.AiId;
    }

    public float GetAttackDistance()
    {
        MonsterConfigItem cfg = MonsterConfig.GetConfigItem(this.actorInfo.ActorConfigId);
        if (cfg == null)
        {
            return 0;
        }
        return cfg.AttackRange;
    }
    
    public float GetAttackInterval()
    {
        MonsterConfigItem cfg = MonsterConfig.GetConfigItem(this.actorInfo.ActorConfigId);
        if (cfg == null)
        {
            return 0;
        }
        return cfg.AttackInterval;
    }
    
    //攻击动画时长
    public float GetAttackAnimDuration()
    {
        return attackAnimDuration;
    }
    
    public void AgentStop()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
        }
    }
    
    public void AgentStart()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = false;
        }
    }
    
    
    //设置目标位置
    public void SetTargetPosition(Vector3 pos)
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(pos);
        }
    }
}