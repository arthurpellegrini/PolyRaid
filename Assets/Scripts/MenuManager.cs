using System.Collections;
using System.Collections.Generic;
using SDD.Events;
using UnityEngine;

public class MenuManager : Manager<MenuManager>
{
	[Header("MenuManager")]

	#region Panels
	[Header("Panels")]
	[SerializeField] GameObject MainMenuGo;
	[SerializeField] GameObject CreditsMenuGo;
	[SerializeField] GameObject PausedMenuGo;
	[SerializeField] GameObject EndgameMenuGo;
	[SerializeField] GameObject DeadMenuGo ;
	[SerializeField] GameObject HUDGo ;

	List<GameObject> AllPanels;
	#endregion

	#region Events' subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
	}
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
		RegisterPanels();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			EscapeButtonHasBeenClicked();
		}
	}
	#endregion

	#region Panel Methods
	void RegisterPanels()
	{
		AllPanels = new List<GameObject>();
		AllPanels.Add(MainMenuGo);
		AllPanels.Add(CreditsMenuGo);
		AllPanels.Add(PausedMenuGo);
		AllPanels.Add(EndgameMenuGo);
		AllPanels.Add(DeadMenuGo);
		AllPanels.Add(HUDGo);
	}

	void OpenPanel(GameObject panel)
	{
		foreach (var item in AllPanels)
			if (item) item.SetActive(item == panel);
	}
	#endregion

	#region UI OnClick Events
	public void EscapeButtonHasBeenClicked()
	{
		EventManager.Instance.Raise(new EscapeButtonClickedEvent());
	}

	public void PlayButtonHasBeenClicked()
	{
		EventManager.Instance.Raise(new PlayButtonClickedEvent());
	}

	public void ResumeButtonHasBeenClicked()
	{
		EventManager.Instance.Raise(new ResumeButtonClickedEvent());
	}		
		
	public void RespawnButtonHasBeenClicked()
	{
		EventManager.Instance.Raise(new RespawnButtonClickedEvent());
	}		
		
	public void CreditsButtonHasBeenClicked()
	{
		EventManager.Instance.Raise(new CreditsButtonClickedEvent());
	}

	public void MainMenuButtonHasBeenClicked()
	{
		EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
	}

	public void QuitButtonHasBeenClicked()
	{
		EventManager.Instance.Raise(new QuitButtonClickedEvent());
	}

	#endregion

	#region Callbacks to GameManager events
	protected override void GameMenu(GameMenuEvent e)
	{
		OpenPanel(MainMenuGo);
	}
		
	protected override void GameCredits(GameCreditsEvent e)
	{
		OpenPanel(CreditsMenuGo);
	}

	protected override void GamePlay(GamePlayEvent e)
	{
		OpenPanel(HUDGo);
	}

	protected override void GamePause(GamePauseEvent e)
	{
		OpenPanel(PausedMenuGo);
	}

	protected override void GameResume(GameResumeEvent e)
	{
		OpenPanel(HUDGo);
	}
		
	protected override void GameRespawn(GameRespawnEvent e)
	{
		OpenPanel(HUDGo);
	}

	protected override void GameDead(GameDeadEvent e)
	{
		OpenPanel(DeadMenuGo);
	}
	#endregion
}