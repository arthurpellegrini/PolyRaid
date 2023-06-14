using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SDD.Events;

public class MenuManager : Manager<MenuManager>
{
    #region Initialize Fields
    [Header("Menus")]
    [SerializeField] private GameObject mainMenuGo;
    [SerializeField] private GameObject creditsMenuGo;
    [SerializeField] private GameObject lobbyMenuGo;
    [SerializeField] private GameObject pausedMenuGo;
    [SerializeField] private GameObject gameOverMenuGo;
    [SerializeField] private GameObject hudGo;
    
    [Header("Lobby")]
    [SerializeField] private GameObject panelPlayerName;
    [SerializeField] private GameObject panelListLobby;
    [SerializeField] private GameObject panelLobby;
    [SerializeField] private GameObject panelCreateLobby;

    private List<GameObject> allMenus;
    private List<GameObject> allLobbyPanels;
    #endregion
    
    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }
    #endregion

    #region Monobehaviour lifecycle
    protected override void Awake()
    {
        base.Awake();
        RegisterMenus();
        RegisterLobbyPanels();
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            EscapeButtonHasBeenClicked();
        }
    }
    #endregion

    #region Menus Methods
    private void RegisterMenus()
    {
        allMenus = new List<GameObject>
        {
            mainMenuGo,
            creditsMenuGo,
            lobbyMenuGo,
            pausedMenuGo,
            gameOverMenuGo,
            hudGo
        };
    }
    private void OpenMenu(GameObject menu)
    {
        foreach (var item in allMenus)
        {
            if (item)
            {
                item.SetActive(item == menu);
            }
        }
    }
    #endregion

    #region Lobby Panels Methods
    private void RegisterLobbyPanels()
    {
        allLobbyPanels = new List<GameObject>
        {
            panelPlayerName,
            panelListLobby,
            panelLobby,
            panelCreateLobby
        };
    }
    private void OpenLobbyPanel(GameObject panel)
    {
        foreach (var item in allLobbyPanels)
        {
            if (item)
            {
                item.SetActive(item == panel);
            }
        }
    }
    #endregion

    #region UI OnClick Events
    public void MainMenuButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
    }
    
    public void CreditsButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new CreditsButtonClickedEvent());
    }

    public void PlayButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new PlayButtonClickedEvent());
    }
    
    public void ChangeNameLobbyButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new ChangeNameLobbyButtonClickedEvent());
    }
    
    public void ReadyLobbyButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new ReadyLobbyButtonClickedEvent());
    }
    
    public void RefreshLobbiesListButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new RefreshLobbiesListButtonClickedEvent());
    }    
    
    public void AddLobbyButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new AddLobbyButtonClickedEvent());
    }    
    
    public void CreateLobbyButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new CreateLobbyButtonClickedEvent());
    }
    
    public void ResumeButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new ResumeButtonClickedEvent());
    }

    public void EscapeButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new EscapeButtonClickedEvent());
    }

    public void QuitButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new QuitButtonClickedEvent());
    }
    #endregion

    #region Callbacks to GameManager events
    protected override void GameMainMenu(GameMainMenuEvent e) { OpenMenu(mainMenuGo); }
    protected override void GameCredits(GameCreditsEvent e) { OpenMenu(creditsMenuGo); }
    protected override void GameLobby(GamePlayEvent e) { OpenMenu(lobbyMenuGo); }
    protected override void GameResume(GameResumeEvent e) { OpenMenu(hudGo); }
    protected override void GamePaused(GamePausedEvent e) { OpenMenu(pausedMenuGo); }
    protected override void GameOver(GameOverEvent e) { OpenMenu(gameOverMenuGo); }
    #endregion
}