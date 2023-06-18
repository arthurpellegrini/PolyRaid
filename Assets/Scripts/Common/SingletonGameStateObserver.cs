using UnityEngine;
using SDD.Events;

public abstract class SingletonGameStateObserver<T> :  Singleton<T>,IEventHandler where T:Component
{
	public virtual void SubscribeEvents()
	{
		EventManager.Instance.AddListener<GameMainMenuEvent>(GameMainMenu);
		EventManager.Instance.AddListener<GameCreditsEvent>(GameCredits);
		EventManager.Instance.AddListener<GameCreateSessionEvent>(GameCreateSession);
		EventManager.Instance.AddListener<GameJoinSessionEvent>(GameJoinSession);
		EventManager.Instance.AddListener<GameResumeEvent>(GameResume);
		EventManager.Instance.AddListener<GamePausedEvent>(GamePaused);
		EventManager.Instance.AddListener<GameOverEvent>(GameOver);
		EventManager.Instance.AddListener<GameErrorEvent>(GameError);
		EventManager.Instance.AddListener<GameChangeMapEvent>(GameChangeMap);
		EventManager.Instance.AddListener<GameTimerChangedEvent>(GameTimerChanged);
		EventManager.Instance.AddListener<PlayerHealthChangedEvent>(PlayerHealthChanged);
		EventManager.Instance.AddListener<PlayerScoreChangedEvent>(PlayerScoreChanged);
		EventManager.Instance.AddListener<SessionIDChangedEvent>(SessionIDChanged);
		EventManager.Instance.AddListener<FpsChangedEvent>(FpsChanged);
		EventManager.Instance.AddListener<PlayerMagChangedEvent>(PlayerMagChanged);
	}

	public virtual void UnsubscribeEvents()
	{
		EventManager.Instance.RemoveListener<GameMainMenuEvent>(GameMainMenu);
		EventManager.Instance.RemoveListener<GameCreditsEvent>(GameCredits);
		EventManager.Instance.RemoveListener<GameCreateSessionEvent>(GameCreateSession);
		EventManager.Instance.RemoveListener<GameJoinSessionEvent>(GameJoinSession);
		EventManager.Instance.RemoveListener<GameResumeEvent>(GameResume);
		EventManager.Instance.RemoveListener<GamePausedEvent>(GamePaused);
		EventManager.Instance.RemoveListener<GameOverEvent>(GameOver);
		EventManager.Instance.RemoveListener<GameChangeMapEvent>(GameChangeMap);
		EventManager.Instance.RemoveListener<GameErrorEvent>(GameError);
		EventManager.Instance.RemoveListener<GameTimerChangedEvent>(GameTimerChanged);
		EventManager.Instance.RemoveListener<PlayerHealthChangedEvent>(PlayerHealthChanged);
		EventManager.Instance.RemoveListener<PlayerScoreChangedEvent>(PlayerScoreChanged);
		EventManager.Instance.RemoveListener<SessionIDChangedEvent>(SessionIDChanged);
		EventManager.Instance.RemoveListener<FpsChangedEvent>(FpsChanged);
		EventManager.Instance.RemoveListener<PlayerMagChangedEvent>(PlayerMagChanged);
	}

	protected override void Awake() { base.Awake(); SubscribeEvents(); }
	public override void OnDestroy() { base.OnDestroy(); UnsubscribeEvents(); }
	
	protected virtual void GameMainMenu(GameMainMenuEvent e) { }
	protected virtual void GameCredits(GameCreditsEvent e) { }
	protected virtual void GameCreateSession(GameCreateSessionEvent e) { }
	protected virtual void GameJoinSession(GameJoinSessionEvent e) { }
	protected virtual void GameResume(GameResumeEvent e) { }
	protected virtual void GamePaused(GamePausedEvent e) { }
	protected virtual void GameOver(GameOverEvent e) { }
	protected virtual void GameChangeMap(GameChangeMapEvent e) { }
	protected virtual void GameError(GameErrorEvent e) { }
	protected virtual void GameTimerChanged(GameTimerChangedEvent e) { }
	protected virtual void PlayerHealthChanged(PlayerHealthChangedEvent e) { }
	protected virtual void PlayerScoreChanged(PlayerScoreChangedEvent e) { }
	protected virtual void SessionIDChanged(SessionIDChangedEvent e) { }
	protected virtual void FpsChanged(FpsChangedEvent e) { }
	protected virtual void PlayerMagChanged(PlayerMagChangedEvent e) { }
}
