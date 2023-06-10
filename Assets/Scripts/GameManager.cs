using System.Collections;
using InputSystem;
using SDD.Events;
using Unity.Netcode;
using UnityEngine;

public enum GameState { Menu, Credits, Playing, Paused, GameOver }

public class GameManager : Manager<GameManager>
{
	private void SetMenuState(bool inMenu)
	{
		PlayerInputController playerInput = FindObjectOfType<PlayerInputController>();
		if (playerInput != null)
		{
			playerInput.inMenu = inMenu;
			playerInput.cursorInputForLook = !inMenu;
		}
	}

	#region Game State
	private GameState _gameState;
	public bool IsPlaying => _gameState == GameState.Playing;
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
	
	#region Time
	void SetTimeScale(float newTimeScale)
	{
		Time.timeScale = newTimeScale;
	}
	#endregion

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
		yield break;
	}
	
	private void GameMainMenu()
	{
		if(MusicLoopsManager.Instance && _gameState != GameState.Credits) MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
		_gameState = GameState.Menu;
		SetTimeScale(1);
		SetMenuState(true);
		EventManager.Instance.Raise(new GameMainMenuEvent());
	}
	
	private void GameCredits()
	{
		_gameState = GameState.Credits;
		SetTimeScale(1);
		SetMenuState(true);
		EventManager.Instance.Raise(new GameCreditsEvent());
	}

	private void GameCreateSession()
	{
		_gameState = GameState.Menu;
		SetTimeScale(1);
		SetMenuState(true);
		
		// Start Host (Without Unity Relay)
		NetworkManager.Singleton.StartHost();
		
		EventManager.Instance.Raise(new GameCreateSessionEvent());
	}

	private void GameJoinSession()
	{
		if(MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
		_gameState = GameState.Playing;
		SetTimeScale(1);
		SetMenuState(false);
		
		// Start Client (Without Unity Relay)
		NetworkManager.Singleton.StartClient();
		
		EventManager.Instance.Raise(new GameJoinSessionEvent());
	}

	private void GameResume()
	{
		_gameState = GameState.Playing;
		SetTimeScale(1);
		SetMenuState(false);
		EventManager.Instance.Raise(new GameResumeEvent());
	}

	private void GamePaused()
	{
		_gameState = GameState.Paused;
		SetTimeScale(1);
		SetMenuState(true);
		EventManager.Instance.Raise(new GamePausedEvent());
	}

	private void GameOver()
	{
		_gameState = GameState.GameOver;
		SetTimeScale(0);
		SetMenuState(false);
		EventManager.Instance.Raise(new GameOverEvent());
	}
	#endregion

	#region Callbacks to Events issued by MenuManager
	private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
	{
		GameMainMenu();
	}
	
	private void CreditsButtonClicked(CreditsButtonClickedEvent e)
	{
		GameCredits();
	}

	private void CreateSessionButtonClicked(CreateSessionButtonClickedEvent e)
	{
		GameCreateSession();		
	}	
	
	private void JoinSessionButtonClicked(JoinSessionButtonClickedEvent e)
	{
		GameJoinSession();		
	}
	
	private void ResumeButtonClicked(ResumeButtonClickedEvent e)
	{
		GameResume();
	}
	
	private void EscapeButtonClicked(EscapeButtonClickedEvent e)
	{
		if (IsPlaying) GamePaused();
		else GameResume();
	}

	private void QuitButtonClicked(QuitButtonClickedEvent e)
	{
		Application.Quit();
	}
	#endregion
}