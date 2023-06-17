using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.InputSystem;
using SDD.Events;
using TMPro;
using UnityEngine.UI;

public class MenuManager : Manager<MenuManager>
{
    [Header("MenuManager")]
    [SerializeField] private GameObject mainMenuGo;
    [SerializeField] private GameObject creditsMenuGo;
    [SerializeField] private GameObject pausedMenuGo;
    [SerializeField] private GameObject gameOverMenuGo;
    [SerializeField] private GameObject gameErrorMenuGo;
    [SerializeField] private GameObject hudGo;
    private List<GameObject> allPanels;
    
    [Header("MainMenu")]
    [SerializeField] private Button _tmpCreateButton;
    [SerializeField] private Button _tmpJoinButton;
    [SerializeField] private TMP_InputField _tmpInputMapId;
    public int GetInputMapId()
    {
        int parsedValue;
        int.TryParse(_tmpInputMapId.text, out parsedValue);
        return parsedValue;
    }
    [SerializeField] private TMP_InputField _tmpInputSessionId;
    public string GetInputSessionId() => _tmpInputSessionId.text;    
    
    [Header("ErrorMenu")]
    [SerializeField] private TMP_Text _tmpErrorTitle;
    [SerializeField] private TMP_Text _tmpErrorDescription;
    
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
        RegisterPanels();
        _tmpJoinButton.interactable = false;
        _tmpCreateButton.interactable = false;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameManager.Instance.IsPlaying) EscapeButtonHasBeenClicked();
        }
    }
    #endregion

    #region Panel Methods
    private void RegisterPanels()
    {
        allPanels = new List<GameObject>
        {
            mainMenuGo,
            creditsMenuGo,
            pausedMenuGo,
            gameOverMenuGo,
            gameErrorMenuGo,
            hudGo
        };
    }

    private void OpenPanel(GameObject panel)
    {
        foreach (var item in allPanels)
        {
            if (item)
            {
                item.SetActive(item == panel);
            }
        }
    }
    #endregion

    #region UI OnClick Events
    public void MainMenuButtonHasBeenClicked() { EventManager.Instance.Raise(new MainMenuButtonClickedEvent()); }
    public void CreditsButtonHasBeenClicked() { EventManager.Instance.Raise(new CreditsButtonClickedEvent()); }
    public void CreateSessionButtonHasBeenClicked() { EventManager.Instance.Raise(new CreateSessionButtonClickedEvent()); }
    public void JoinSessionButtonHasBeenClicked() { EventManager.Instance.Raise(new JoinSessionButtonClickedEvent()); }
    public void ResumeButtonHasBeenClicked() { EventManager.Instance.Raise(new ResumeButtonClickedEvent()); }
    public void EscapeButtonHasBeenClicked() { EventManager.Instance.Raise(new EscapeButtonClickedEvent()); }
    public void QuitButtonHasBeenClicked() { EventManager.Instance.Raise(new QuitButtonClickedEvent()); }
    public void OnMapIDValueChanged(string code) { CheckInputFieldsToSetButtonsInteractable(); }
    public void OnCodeSessionValueChanged(string code) { CheckInputFieldsToSetButtonsInteractable(); }

    private void CheckInputFieldsToSetButtonsInteractable()
    {
        int mapId = GetInputMapId();
        if (mapId > 0 && mapId <= 4) 
        {
            _tmpCreateButton.interactable = true;
            _tmpJoinButton.interactable = GetInputSessionId().Length == 6; // Solve Length Issues
        }
        else
        {
            _tmpCreateButton.interactable = _tmpJoinButton.interactable = false;
        }
    }
    
    #endregion

    #region Callbacks to GameManager events
    protected override void GameMainMenu(GameMainMenuEvent e) { OpenPanel(mainMenuGo); }
    protected override void GameCredits(GameCreditsEvent e) { OpenPanel(creditsMenuGo); }
    protected override void GameCreateSession(GameCreateSessionEvent e) { OpenPanel(hudGo); }
    protected override void GameJoinSession(GameJoinSessionEvent e) { OpenPanel(hudGo); }
    protected override void GameResume(GameResumeEvent e) { OpenPanel(hudGo); }
    protected override void GamePaused(GamePausedEvent e) { OpenPanel(pausedMenuGo); }
    protected override void GameOver(GameOverEvent e) { OpenPanel(gameOverMenuGo); }
    protected override void GameError(GameErrorEvent e)
    {
        _tmpErrorTitle.text = e.eErrorTitle;
        _tmpErrorDescription.text = e.eErrorDescription;
        OpenPanel(gameErrorMenuGo);
    }
    #endregion
}