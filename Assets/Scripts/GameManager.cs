using System;
using System.Collections;
using System.Threading.Tasks;
using InputSystem;
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
    private GameState _gameState;
    public bool IsPlaying => _gameState == GameState.Playing;

    private UnityTransport _transport;
    private PlayerInputController _playerInput;
    
    [SerializeField] private float _GameOverDuration = 60;
    private float _timer;
    private int _score;
    private int _health;
    
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
    }
    
    protected override IEnumerator Start()
    {
        yield return base.Start();
        _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        _playerInput = FindObjectOfType<PlayerInputController>();
    }

    protected void Update()
    {
        // SetSessionInfo();
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
        EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.AddListener<ChangeNameLobbyButtonClickedEvent>(ChangeNameLobbyButtonClicked);
        EventManager.Instance.AddListener<ReadyLobbyButtonClickedEvent>(ReadyLobbyButtonClicked);
        EventManager.Instance.AddListener<RefreshLobbiesListButtonClickedEvent>(RefreshLobbiesListButtonClicked);
        EventManager.Instance.AddListener<AddLobbyButtonClickedEvent>(AddLobbyButtonClicked);
        EventManager.Instance.AddListener<CreateLobbyButtonClickedEvent>(CreateLobbyButtonClicked);
        EventManager.Instance.AddListener<CreditsButtonClickedEvent>(CreditsButtonClicked);
        EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.RemoveListener<ChangeNameLobbyButtonClickedEvent>(ChangeNameLobbyButtonClicked);
        EventManager.Instance.RemoveListener<ReadyLobbyButtonClickedEvent>(ReadyLobbyButtonClicked);
        EventManager.Instance.RemoveListener<RefreshLobbiesListButtonClickedEvent>(RefreshLobbiesListButtonClicked);
        EventManager.Instance.RemoveListener<AddLobbyButtonClickedEvent>(AddLobbyButtonClicked);
        EventManager.Instance.RemoveListener<CreateLobbyButtonClickedEvent>(CreateLobbyButtonClicked);
        EventManager.Instance.RemoveListener<CreditsButtonClickedEvent>(CreditsButtonClicked);
        EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);
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
    
    // void SetSessionInfo()
    // {
    //     // _fps = (int)(1f / Time.unscaledDeltaTime);
    //     _fps = SetFrameRate();
    //     _ping = _transport ? _transport.DebugSimulator.PacketDelayMS : -1;
    //     EventManager.Instance.Raise(new SessionStatisticsChangedEvent() { eSessionID = _sessionID, eFps = _fps, ePing = _ping});
    // }

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
        if (MusicLoopsManager.Instance && _gameState != GameState.Menu) MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
        _gameState = GameState.Menu;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GameMainMenuEvent());
    }
    
    private void GameLobby()
    {
        _gameState = GameState.Menu;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GameLobbyEvent());
    }
    
    private void UpdatePlayerName()
    {
        _gameState = GameState.Menu;
        SetTimeScale(1);
        
    }
    
    private void SetPlayerLobbyStateToReady()
    {
        _gameState = GameState.Menu;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GameCreditsEvent());
    }
    
    private void RefreshLobbiesList()
    {
        _gameState = GameState.Menu;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GameCreditsEvent());
    }
    
    private void AddLobby()
    {
        _gameState = GameState.Menu;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GameCreditsEvent());
    }
    
    private void CreateLobby()
    {
        _gameState = GameState.Menu;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GameCreditsEvent());
    }
    
    

    private void GameCredits()
    {
        _gameState = GameState.Menu;
        SetTimeScale(1);
        EventManager.Instance.Raise(new GameCreditsEvent());
    }

    // private void GamePlay()
    // {
    //     if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
    //     _gameState = GameState.Playing;
    //     SetTimeScale(1);
    //
    //     EventManager.Instance.Raise(new GamePlayEvent());
    // }

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
    private void PlayButtonClicked(PlayButtonClickedEvent e) { GameLobby(); }
    private void ChangeNameLobbyButtonClicked(ChangeNameLobbyButtonClickedEvent e) { UpdatePlayerName(); }
    private void ReadyLobbyButtonClicked(ReadyLobbyButtonClickedEvent e) { SetPlayerLobbyStateToReady(); }
    private void RefreshLobbiesListButtonClicked(RefreshLobbiesListButtonClickedEvent e) { RefreshLobbiesList(); }
    private void AddLobbyButtonClicked(AddLobbyButtonClickedEvent e) { AddLobby(); }
    private void CreateLobbyButtonClicked(CreateLobbyButtonClickedEvent e) { CreateLobby(); }
    private void CreditsButtonClicked(CreditsButtonClickedEvent e) { GameCredits(); }
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