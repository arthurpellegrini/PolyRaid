using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Manager<LevelManager>
{
	private MapState _mapState = MapState.Test;
	public int GetMapState => (int)_mapState;
	
	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		RegisterMaps();
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
	
	#region GameObjects
	[SerializeField] private GameObject _playerPrefab;
	[SerializeField] private Map _testPrefab;
	[SerializeField] private GameObject _esieeItPrefab;
	[SerializeField] private GameObject _yassinePrefab;
	[SerializeField] private GameObject _clementPrefab;

	private List<Map> allMapsPrefab;
	
	private Map _currentMapGo;
	private List<Transform> _spawnPoints;
	private int _randomSpawnPoint=0;

	private void RegisterMaps()
	{
		allMapsPrefab = new List<Map>
		{
			_testPrefab,
			// _esieeItPrefab,
			// _yassinePrefab,
			// _clementPrefab
		};
	}
	
	public Transform GetRandomSpawnPoint()
	{
		if (_spawnPoints.Count == 0)
		{
			Debug.LogError("La liste des points de spawn est vide !");
			return null;
		}
		return _spawnPoints[_randomSpawnPoint%_spawnPoints.Count];
	}

	private void InitMap()
	{
		_mapState = 0;
		// _mapState = (MapState)Random.Range(0, 2); // Synchronize with server that radom each new session
		_currentMapGo = Instantiate(allMapsPrefab[(int)_mapState], Vector3.zero, Quaternion.identity);
		_spawnPoints = _currentMapGo.SpawnPoints;
	}

	private void DestroyMap()
	{
		if(_currentMapGo) Destroy(_currentMapGo);
	}
	#endregion
	
	protected override void GameMainMenu(GameMainMenuEvent e)
	{
		DestroyMap();
	}
	
	protected override void GameCredits(GameCreditsEvent e)
	{

	}

	protected override void GameCreateSession(GameCreateSessionEvent e)
	{
		InitMap();
	}

	protected override void GameJoinSession(GameJoinSessionEvent e)
	{
		InitMap();
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