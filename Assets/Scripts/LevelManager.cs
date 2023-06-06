using System.Collections;

public class LevelManager : Manager<LevelManager>
{
	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion
	
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
	}

	// Méthodes caméra Player + Environnement
	protected override void GameMainMenu(GameMainMenuEvent e)
	{

	}
	
	protected override void GameCredits(GameCreditsEvent e)
	{

	}

	protected override void GameCreateSession(GameCreateSessionEvent e)
	{

	}

	protected override void GameJoinSession(GameJoinSessionEvent e)
	{

	}

	protected override void GameResume(GameResumeEvent e)
	{

	}

	protected override void GamePaused(GamePausedEvent e)
	{

	}

	protected override void GameOver(GameOverEvent e)
	{

	}
	
	protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
		
	}
}