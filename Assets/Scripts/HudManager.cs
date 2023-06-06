using System.Collections;

public class HudManager : Manager<HudManager>
{

	//[Header("HudManager")]
	#region Labels & Values
	// TO DO
	#endregion

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion

	#region Callbacks to GameManager events
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
	#endregion

}