public class CharacterDBService :DBModuleBase
{
    private readonly IUnitOfWork _uow;

    public CharacterDBService(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    #region 登录

    //登录
    public async Task<LoginUIdResultCallBack> Login(string account, string password )
    {
        try
        {
            _uow.BeginTransaction();
            int uId = await _uow.PlayerRepository.CheckPlayerName(account);
            if (uId == -1)
            {
                // Account does not exist 新建用户
                uId = await _uow.PlayerRepository.CreatePlayer(account, password);
                //默认添加俩个宠物
                await _uow.PetRepository.AddPetAsync(uId, 1 ,true);
                await _uow.PetRepository.AddPetAsync(uId, 2 ,true);
                _uow.Commit();
            }
            else
            {
                // Account exists 登录用户
                bool isExist = await _uow.PlayerRepository.CheckUid(uId);
                if (!isExist)
                {
                    // 密码错误
                    return new LoginUIdResultCallBack()
                    {
                        IsSuccess = false,
                        Message = "密码错误",
                        UId = -1
                    };
                }
            }
            return new LoginUIdResultCallBack()
            {
                IsSuccess = true,
                Message = "登录成功",
                UId = uId
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // 事务回滚
            _uow.Rollback();
            throw;
        }
    }
    
    //uId登录
    public async Task<LoginUIdResultCallBack> Login(int uId)
    {
        try
        {
            _uow.BeginTransaction();
            bool isExist = await _uow.PlayerRepository.CheckUid(uId);
            if (!isExist)
            {
                // Account does not exist 新建用户
                return new LoginUIdResultCallBack()
                {
                    IsSuccess = false,
                    Message = "用户不存在",
                    UId = -1
                };
            }
            return new LoginUIdResultCallBack()
            {
                IsSuccess = true,
                Message = "登录成功",
                UId = uId
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion
    
    #region 角色信息
    //获取角色信息
    public async Task<PlayerDB> GetCharacterInfo(int uId)
    {
        try
        {
            _uow.BeginTransaction();
            PlayerDB playerInfo = await _uow.PlayerRepository.GetPlayerInfo(uId);
            if (playerInfo == null)
            {
                // 角色不存在
                return null;
            }

            playerInfo.Pets = await _uow.PetRepository.GetAllPetsAsync(uId);
            return playerInfo;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    #endregion

    #region 宠物逻辑

    public async Task<int> AddPet(int belongTo,int petConfig,bool bBattle)
    {
        try
        {
            _uow.BeginTransaction();
            int petId = await _uow.PetRepository.AddPetAsync( belongTo, petConfig, bBattle);
            _uow.Commit();
            return petId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion

}