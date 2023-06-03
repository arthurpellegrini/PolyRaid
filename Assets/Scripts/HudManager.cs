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
	protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
		//TO DO
	}
	#endregion

}