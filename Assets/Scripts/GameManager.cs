using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private static GameManager _instance;

    private NetworkManager networkManager;
    private UIManager uiManager;
    private AppEventDispatcher appEventDispatcher;
    private GameStateMachine gameStateMachine;
    private GameSyncActorManager gameSyncActorManager;

    struct TestInfo
    {
        public int hp ;
        public int mp;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        TestInfo testInfo = new TestInfo();
        testInfo.hp = 100;
        testInfo.mp = 100;
        string json = JsonUtility.ToJson(testInfo);
        Debug.Log("[Json] TestInfo: " + json);
        TestInfo testInfo2 = JsonUtility.FromJson<TestInfo>(json);
        Debug.Log("[Json] TestInfo2: " + testInfo2.hp + " " + testInfo2.mp);
    }
    
    public static GameManager Instance => _instance;
    
    private void Start()
    {
        ClientFactoryRegisterHelper.Register();

        networkManager = new NetworkManager();
        networkManager.Init();
        
        uiManager = new UIManager();
        uiManager.Init();
        
        appEventDispatcher = new AppEventDispatcher();
        appEventDispatcher.Init();
        
        gameStateMachine = new GameStateMachine();
        gameStateMachine.Init();
        
        gameSyncActorManager = GetComponent<GameSyncActorManager>();
        gameSyncActorManager.Init();
        
        RoomManager.Instance.Init();
        GOtPoolManager.Instance.Init();
    }
    
    public static NetworkManager GetNetworkManager()
    {
        return _instance.networkManager;
    }
    
    public static UIManager GetUIManager()
    {
        return _instance.uiManager;
    }
    
    public static AppEventDispatcher GetAppEventDispatcher()
    {
        return _instance.appEventDispatcher;
    }
    
    public static GameStateMachine GetGameStateMachine()
    {
        return _instance.gameStateMachine;
    }
    
    public static GameSyncActorManager GetGameSyncActorManager()
    {
        return _instance.gameSyncActorManager;
    }
    
    private void FixedUpdate()
    {
        if (gameSyncActorManager != null)
        {
            gameSyncActorManager.DoFixedUpdate();
        }

        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.DoFixedUpdate();
        }
    }
    
    private void Update()
    {
        if (uiManager != null)
        {
            uiManager.DoUpdate(Time.deltaTime);
        }
        if (gameStateMachine != null)
        {
            gameStateMachine.DoUpdate(Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        if (appEventDispatcher != null)
        {
            appEventDispatcher.Dispose();
        }
        if (gameStateMachine != null)
        {
            gameStateMachine.Dispose();
        }
    }
}