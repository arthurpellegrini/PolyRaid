using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Manager<LevelManager>
{
	// Synchronize Server and Client on the MapId (Not Working because require that the Client
	// Spawn before the variable is sync)
	public NetworkVariable<int> _currentMapId = new NetworkVariable<int>(0);
	
	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		RegisterMaps();
		yield break;
	}
	#endregion

	#region GameObjects
	[SerializeField] private Map _testPrefab;
	[SerializeField] private Map _esieeItPrefab;
	[SerializeField] private Map _yassinePrefab;
	[SerializeField] private Map _clementPrefab;

	private List<Map> allMapsPrefab;
	
	private Map _currentMap;
	private NetworkObject _currentMapNo;
	private List<Transform> _spawnTransforms;
	private int _randomSpawnNumber;

	private void RegisterMaps()
	{
		allMapsPrefab = new List<Map>
		{
			_testPrefab,
			_esieeItPrefab,
			_yassinePrefab,
			_clementPrefab
		};
	}

	public void RandomizeSpawnPoint()
	{
		_randomSpawnNumber = Random.Range(0, 65536);
	}
	
	public Transform GetRandomSpawn()
	{
		RandomizeSpawnPoint();
		Transform spawn = _spawnTransforms[_randomSpawnNumber % _spawnTransforms.Count];
		// Debug.Log(_randomSpawnNumber.ToString() + ";" + spawn + ";" + spawn.position);
		return spawn;
	}

	private void InitMap()
	{
		_currentMapId.Value = MenuManager.Instance.GetInputMapId()-1; // MapId will be in [1..3] but the list start at 0
		if (IsHost)
		{
			_currentMap = Instantiate(allMapsPrefab[_currentMapId.Value], Vector3.zero, Quaternion.identity);
			_currentMapNo = _currentMap.GetComponent<NetworkObject>();
			_currentMapNo.Spawn();
		}
		_spawnTransforms = allMapsPrefab[_currentMapId.Value].SpawnPoints;
		// Debug.Log("MapID:" + _currentMapId.Value);
	}
	#endregion

	protected override void GameMainMenu(GameMainMenuEvent e)
	{

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

	// protected override void GameChangeMap(GameChangeMapEvent e)
	// {
	// 	_currentMapId.Value++;
	// 	InitMap();
	// }
	
	protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
		
	}
}
