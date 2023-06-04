using System.Collections;
using Cinemachine;
using InputSystem;
using SDD.Events;
using UnityEngine;

public enum GameState { gameMenu, gameCredits, gamePlay, gameNextLevel, gamePause, gameOver, gameVictory }

public class GameManager : Manager<GameManager>
{
	#region GameObjects (Player & Environnement --> Scene)
	[SerializeField] private CinemachineVirtualCamera VirtualCamera;
	[SerializeField] private GameObject PlayerPrefab;
	[SerializeField] private GameObject EnvironmentPrefab;

	private GameObject PlayerGo;
	private GameObject EnvironementGo;

	private void InitScene()
	{
		EnvironementGo = Instantiate(EnvironmentPrefab, Vector3.zero, Quaternion.identity);
		PlayerGo = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
		VirtualCamera.Follow = PlayerGo.transform;
	}

	private void DestroyScene()
	{
		if(PlayerGo) Destroy(PlayerGo);
		if(EnvironementGo) Destroy(EnvironementGo);
	}
	
	private void SetMenuState(bool inMenu)
	{
		PlayerInputController playerInput = FindObjectOfType<PlayerInputController>();
		if (playerInput != null)
		{
			playerInput.inMenu = inMenu;
			playerInput.SetCursorVisible(inMenu);
		}
	}
	#endregion
	
	#region Game State
	private GameState m_GameState;
	public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }
	#endregion

	#region Lives
	[Header("GameManager")]
	[SerializeField]
	private int m_NStartLives;

	private int m_NLives;
	public int NLives { get { return m_NLives; } }
	void DecrementNLives(int decrement)
	{
		SetNLives(m_NLives - decrement);
	}

	void SetNLives(int nLives)
	{
		m_NLives = nLives;
		EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eScore = m_Score});
	}
	#endregion
	
	#region Score
	private float m_Score;
	public float Score
	{
		get { return m_Score; }
		set
		{
			m_Score = value;
			BestScore = Mathf.Max(BestScore, value);
		}
	}

	public float BestScore
	{
		get { return PlayerPrefs.GetFloat("BEST_SCORE", 0); }
		set { PlayerPrefs.SetFloat("BEST_SCORE", value); }
	}

	void IncrementScore(float increment)
	{
		SetScore(m_Score + increment);
	}

	void SetScore(float score, bool raiseEvent = true)
	{
		Score = score;
		
		if (raiseEvent)
			EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eScore = m_Score });
	}
	#endregion

	#region Time
	void SetTimeScale(float newTimeScale)
	{
		Time.timeScale = newTimeScale;
	}
	#endregion


	#region Events' subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();
			
		//MainMenuManager
		EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
		EventManager.Instance.AddListener<CreditsButtonClickedEvent>(CreditsButtonClicked);
		EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
		EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
		EventManager.Instance.AddListener<RespawnButtonClickedEvent>(RespawnButtonClicked);
		EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
		EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);

		//Score Item
		EventManager.Instance.AddListener<ScoreItemEvent>(ScoreHasBeenGained);
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();

		//MainMenuManager
		EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
		EventManager.Instance.RemoveListener<CreditsButtonClickedEvent>(CreditsButtonClicked);
		EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
		EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
		EventManager.Instance.RemoveListener<RespawnButtonClickedEvent>(RespawnButtonClicked);
		EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
		EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);

		//Score Item
		EventManager.Instance.RemoveListener<ScoreItemEvent>(ScoreHasBeenGained);
	}
	#endregion

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		Menu();
		InitNewGame(); // essentiellement pour que les statistiques du jeu soient mise à jour en HUD
		yield break;
	}
	#endregion

	#region Game flow & Gameplay
	//Game initialization
	void InitNewGame(bool raiseStatsEvent = true)
	{
		SetScore(0);
	}
	#endregion

	#region Callbacks to events issued by Score items
	private void ScoreHasBeenGained(ScoreItemEvent e)
	{
		if (IsPlaying)
			IncrementScore(e.eScore);
	}
	#endregion

	#region Callbacks to Events issued by MenuManager
	private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
	{
		Menu();
	}

	private void PlayButtonClicked(PlayButtonClickedEvent e)
	{
		Play();
	}
	
	private void CreditsButtonClicked(CreditsButtonClickedEvent e)
	{
		Credits();
	}

	private void RespawnButtonClicked(RespawnButtonClickedEvent e)
	{
		Respawn();
	}
	
	private void ResumeButtonClicked(ResumeButtonClickedEvent e)
	{
		Resume();
	}

	private void EscapeButtonClicked(EscapeButtonClickedEvent e)
	{
		if (IsPlaying) Pause();
	}

	private void QuitButtonClicked(QuitButtonClickedEvent e)
	{
		Application.Quit();
	}
	#endregion

	#region GameState methods
	private void Menu()
	{
		DestroyScene();
		SetMenuState(true);
		SetTimeScale(1);
		if(MusicLoopsManager.Instance && m_GameState != GameState.gameCredits)MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
		m_GameState = GameState.gameMenu;
		EventManager.Instance.Raise(new GameMenuEvent());
	}
	
	private void Credits()
	{
		SetMenuState(true);
		SetTimeScale(1);
		m_GameState = GameState.gameCredits;
		EventManager.Instance.Raise(new GameCreditsEvent());
	}

	private void Play()
	{
		InitScene();
		InitNewGame();
		SetMenuState(false);
		SetTimeScale(1);
		m_GameState = GameState.gamePlay;

		if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
		EventManager.Instance.Raise(new GamePlayEvent());
	}

	private void Pause()
	{
		if (!IsPlaying) return;
		SetMenuState(true);
		SetTimeScale(0);
		m_GameState = GameState.gamePause;
		EventManager.Instance.Raise(new GamePauseEvent());
	}

	private void Resume()
	{
		if (IsPlaying) return;
		SetMenuState(false);
		SetTimeScale(1);
		m_GameState = GameState.gamePlay;
		EventManager.Instance.Raise(new GameResumeEvent());
	}
	
	private void Respawn()
	{
		if (IsPlaying) return;
		SetMenuState(true);
		SetTimeScale(1);
		m_GameState = GameState.gamePlay;
		EventManager.Instance.Raise(new GameRespawnEvent());
	}

	private void Over()
	{
		SetMenuState(true);
		SetTimeScale(0);
		m_GameState = GameState.gameOver;
		EventManager.Instance.Raise(new GameDeadEvent());
		if(SfxManager.Instance) SfxManager.Instance.PlaySfx2D(Constants.GAMEOVER_SFX);
	}
	#endregion
}