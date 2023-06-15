using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject hudGo;
    private List<GameObject> allPanels;
    
    [SerializeField] private TMP_InputField _tmpInputField;
    public string getInputField() { return _tmpInputField.text; }
    public void setInputField(string newText) { _tmpInputField.text = newText; }
    [SerializeField] private Button _tmpJoinButton;
    
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
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            EscapeButtonHasBeenClicked();
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
    public void MainMenuButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
    }
    
    public void CreditsButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new CreditsButtonClickedEvent());
    }

    public void CreateSessionButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new CreateSessionButtonClickedEvent());
    }
    
    public void JoinSessionButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new JoinSessionButtonClickedEvent());
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

    public void OnCodeSessionValueChanged(string code)
    {
        _tmpJoinButton.interactable = code.Length >= 6;
    }
    #endregion

    #region Callbacks to GameManager events
    protected override void GameMainMenu(GameMainMenuEvent e)
    {
        OpenPanel(mainMenuGo);
    }
    
    protected override void GameCredits(GameCreditsEvent e)
    {
        OpenPanel(creditsMenuGo);
    }

    protected override void GameCreateSession(GameCreateSessionEvent e)
    {
        OpenPanel(hudGo);
    }

    protected override void GameJoinSession(GameJoinSessionEvent e)
    {
        OpenPanel(hudGo);
    }

    protected override void GameResume(GameResumeEvent e)
    {
        OpenPanel(hudGo);
    }

    protected override void GamePaused(GamePausedEvent e)
    {
        OpenPanel(pausedMenuGo);
    }

    protected override void GameOver(GameOverEvent e)
    {
        OpenPanel(gameOverMenuGo);
    }
    #endregion
}