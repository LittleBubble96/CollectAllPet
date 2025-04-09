
//注册工作单元 方便以后扩展数据库
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// 提交工作单元
    /// </summary>
    void Commit();

    /// <summary>
    /// 回滚工作单元
    /// </summary>
    void Rollback();
    
    /// <summary>
    /// 开始工作单元
    /// </summary>
    void BeginTransaction();
    
    PlayerRepository PlayerRepository { get; }
    PetRepository PetRepository { get; }
    
}