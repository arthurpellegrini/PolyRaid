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

	protected override void GamePlay(GamePlayEvent e)
	{
	}

	protected override void GameMenu(GameMenuEvent e)
	{
	}
}