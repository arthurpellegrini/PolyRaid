using System.Collections;
using Cinemachine;
using UnityEngine;

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
	
	#region GameObjects (Player & Environnement --> Scene)
	[SerializeField] private CinemachineVirtualCamera VirtualCamera;
	[SerializeField] private GameObject PlayerPrefab;
	[SerializeField] private GameObject EnvironmentPrefab;

	private GameObject PlayerGo;
	private GameObject EnvironementGo;

	private void InitScene()
	{
		EnvironementGo = Instantiate(EnvironmentPrefab, Vector3.zero, Quaternion.identity);
		// PlayerGo = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
		// VirtualCamera.Follow = PlayerGo.transform;
	}

	private void DestroyScene()
	{
		// if(PlayerGo) Destroy(PlayerGo);
		if(EnvironementGo) Destroy(EnvironementGo);
	}
	#endregion
	
	protected override void GameMainMenu(GameMainMenuEvent e)
	{
		DestroyScene();
	}
	
	protected override void GameCredits(GameCreditsEvent e)
	{

	}

	protected override void GameCreateSession(GameCreateSessionEvent e)
	{
		InitScene(); //TODO : REMOVE IF USE RELAY
	}

	protected override void GameJoinSession(GameJoinSessionEvent e)
	{
		InitScene();
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