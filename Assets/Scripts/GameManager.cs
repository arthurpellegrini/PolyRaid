using System;
using System.Collections;
using System.Threading.Tasks;
using SDD.Events;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public enum GameState
{
    Menu,
    Playing,
    Paused,
    GameOver
}

public class GameManager : Manager<GameManager>
{
    private GameState _gameState = GameState.Playing;
    public bool IsPlaying => _gameState == GameState.Playing;

    private UnityTransport _transport;
    
    [SerializeField] private float _GameOverDuration = 60;
    private float _timer;
    private int _score;
    private int _health;
    
    private string _sessionID;
    private int _lastFrameIndex;
    private float[] _frameDeltaTimeArray = new float[30];
    private int _fps;
    private int _ping;

    #region Manager implementation
    void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }

    protected override IEnumerator InitCoroutine()
    {
        EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
        yield break;
    }
    
    protected override async void Awake()
    {
        base.Awake();
        await Authenticate();
    }
    
    protected override IEnumerator Start()
    {
        yield return base.Start();
        _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
    }

    protected void Update()
    {
        SetSessionInfo();
        if (IsPlaying)
        {
            SetTimer(_timer - Time.deltaTime);
        }
    }
    #endregion
    
    #region Events' subscription

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.AddListener<CreditsButtonClickedEvent>(CreditsButtonClicked);
        EventManager.Instance.AddListener<CreateSessionButtonClickedEvent>(CreateSessionButtonClicked);
        EventManager.Instance.AddListener<JoinSessionButtonClickedEvent>(JoinSessionButtonClicked);
        EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.RemoveListener<CreditsButtonClickedEvent>(CreditsButtonClicked);
        EventManager.Instance.RemoveListener<CreateSessionButtonClickedEvent>(CreateSessionButtonClicked);
        EventManager.Instance.RemoveListener<JoinSessionButtonClickedEvent>(JoinSessionButtonClicked);
        EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);
    }

    #endregion
    
    #region Unity Netcode + Relay/Lobby
    private static async Task Authenticate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateMultiplayerRelay()
    {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(8);
        _sessionID = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        MenuManager.Instance.setInputField(_sessionID);
        _transport.SetRelayServerData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key,
            a.ConnectionData);

        NetworkManager.Singleton.StartHost();
    }

    public async void JoinSession()
    {
        try
        {
            _sessionID = MenuManager.Instance.getInputField();
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(_sessionID);

            _transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key,
                a.ConnectionData, a.HostConnectionData);

            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            Debug.LogError("Error when trying to join the session " + e.Message);
        }
    }
    #endregion
    
    #region GameStatistics
    int SetFrameRate()
    {
        _frameDeltaTimeArray[_lastFrameIndex] = Time.deltaTime;
        _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;
        float total = 0f;
        foreach (float deltaTime in _frameDeltaTimeArray) total += deltaTime;
        return (int)(_frameDeltaTimeArray.Length / total);
    }
    
    void SetSessionInfo()
    {
        // _fps = (int)(1f / Time.unscaledDeltaTime);
        _fps = SetFrameRate();
        EventManager.Instance.Raise(new SessionStatisticsChangedEvent() { eSessionID = _sessionID, eFps = _fps });
    }

    void SetTimer(float newTimer)
    {
        _timer = Mathf.Max(newTimer, 0);
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eTimer = newTimer });

        if (_timer == 0) GameOver();
    }
    #endregion

    #region GameManager Functions
    private void GameMainMenu()
    {
        if (MusicLoopsManager.Instance && _gameState != GameState.Menu)
            MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
        _gameState = GameState.Menu;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GameMainMenuEvent());
    }

    private void GameCredits()
    {
        _gameState = GameState.Menu;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GameCreditsEvent());
    }

    private void GameCreateSession()
    {
        _gameState = GameState.Playing;
        SetTimeScale(1);
        
        SetTimer(_GameOverDuration); // TODO : Test Timer HUD
        
        CreateMultiplayerRelay();

        EventManager.Instance.Raise(new GameCreateSessionEvent());
    }

    private void GameJoinSession()
    {
        if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
        _gameState = GameState.Playing;
        SetTimeScale(1);
        
        JoinSession();

        EventManager.Instance.Raise(new GameJoinSessionEvent());
    }

    private void GameResume()
    {
        _gameState = GameState.Playing;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GameResumeEvent());
    }

    private void GamePaused()
    {
        _gameState = GameState.Paused;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GamePausedEvent());
    }

    private void GameOver()
    {
        _gameState = GameState.GameOver;
        SetTimeScale(0);
        EventManager.Instance.Raise(new GameOverEvent());
    }
    #endregion

    #region Callbacks to Events issued by MenuManager
    private void MainMenuButtonClicked(MainMenuButtonClickedEvent e) { GameMainMenu(); }
    private void CreditsButtonClicked(CreditsButtonClickedEvent e) { GameCredits(); }
    private void CreateSessionButtonClicked(CreateSessionButtonClickedEvent e) { GameCreateSession(); }
    private void JoinSessionButtonClicked(JoinSessionButtonClickedEvent e) { GameJoinSession(); }
    private void ResumeButtonClicked(ResumeButtonClickedEvent e) { GameResume(); }
    private void EscapeButtonClicked(EscapeButtonClickedEvent e) { if (IsPlaying) GamePaused(); else GameResume(); }
    private void QuitButtonClicked(QuitButtonClickedEvent e) { Application.Quit(); }
    #endregion
    
    void EnemyHasBeenHit(EnemyHasBeenHitEvent e)
    {
        // IScore score = e.eEnemyGO.GetComponent<IScore>();
        // if (score != null)
        // {
        //     SetScore(m_Score + score.Score);
        // }
    }
}