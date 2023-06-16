using System;
using System.Collections;
using System.Threading.Tasks;
using SDD.Events;
using Unity.Netcode;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    private GameState _gameState = GameState.Playing;
    public bool IsPlaying => _gameState == GameState.Playing;

    private RelayHostData _relayHostData;
    private RelayJoinData _relayJoinData;
    // private UnityTransport _transport;
    
    // [SerializeField] private float _GameOverDuration = 60;
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

    protected void Update()
    {
        if (IsPlaying)
        {
            SetSessionInfo();
            // SetTimer(_timer - Time.deltaTime);
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

    #region GameStatistics
    private int SetFrameRate()
    {
        _frameDeltaTimeArray[_lastFrameIndex] = Time.deltaTime;
        _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;
        float total = 0f;
        foreach (float deltaTime in _frameDeltaTimeArray) total += deltaTime;
        return Mathf.RoundToInt(_frameDeltaTimeArray.Length / total);
    }
    
    private void SetSessionInfo()
    {
        if (IsHost) 
        {
            EventManager.Instance.Raise(new SessionStatisticsChangedEvent()
            {
                eSessionID = _relayHostData.JoinCode, 
                eFps = SetFrameRate()
            });
        }
        else if (IsClient)
        {
            EventManager.Instance.Raise(new SessionStatisticsChangedEvent()
            {
                eSessionID = _relayJoinData.JoinCode, 
                eFps = SetFrameRate()
            });
        }
    }

    // private void SetTimer(float newTimer)
    // {
    //     _timer = Mathf.Max(newTimer, 0);
    //     EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eTimer = newTimer });
    //
    //     if (_timer == 0) GameOver();
    // }
    #endregion

    #region GameManager Functions
    private void GameMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
        if (_gameState != GameState.Menu) MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
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

    private async void GameCreateSession()
    {
        try
        {
            // if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
            _gameState = GameState.Playing;
            SetTimeScale(1);

            // SetTimer(_GameOverDuration); // TODO : Test Timer HUD

            _relayHostData = await RelayManager.Instance.SetupRelay();
            NetworkManager.Singleton.StartHost();
            EventManager.Instance.Raise(new GameCreateSessionEvent());
        } 
        catch (Exception e)
        {
            _gameState = GameState.Menu;
            EventManager.Instance.Raise(
                new GameErrorEvent() { eErrorTitle = "Setup Relay Error", eErrorDescription = e.Message });
        }
    }

    private async void GameJoinSession()
    {
        try {
            // if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
            _gameState = GameState.Playing;
            SetTimeScale(1);

            _relayJoinData = await RelayManager.Instance.JoinRelay(MenuManager.Instance.GetInputSessionId());
            NetworkManager.Singleton.StartClient();
            EventManager.Instance.Raise(new GameJoinSessionEvent());
        } 
        catch (Exception e)
        {
            _gameState = GameState.Menu;
            EventManager.Instance.Raise(
                new GameErrorEvent() { eErrorTitle = "Join Relay Error", eErrorDescription = e.Message });
        }
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
    
    // void EnemyHasBeenHit(EnemyHasBeenHitEvent e)
    // {
        // IScore score = e.eEnemyGO.GetComponent<IScore>();
        // if (score != null)
        // {
        //     SetScore(m_Score + score.Score);
        // }
    // }
}