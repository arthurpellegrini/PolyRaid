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
	private void InitHostMap()
	{
		_currentMap = Instantiate(allMapsPrefab[_currentMapId.Value], Vector3.zero, Quaternion.identity);
		_currentMapNo = _currentMap.GetComponent<NetworkObject>();
		_currentMapNo.Spawn();
	}
	
	public void RandomizeSpawnPoints() { _randomSpawnNumber = Random.Range(0, 65536); }
	public Transform GetRandomSpawn()
	{
		RandomizeSpawnPoints();
		return _spawnTransforms[_randomSpawnNumber % _spawnTransforms.Count];
	}
	public void UpdateSpawnTransforms() { _spawnTransforms = allMapsPrefab[_currentMapId.Value].SpawnPoints; }
	#endregion

	protected override void GameCreateSession(GameCreateSessionEvent e)
	{
		_currentMapId.Value = MenuManager.Instance.GetInputMapId()-1; // MapId will be in [1..4] but the list start at 0
		if (IsHost) InitHostMap();
		UpdateSpawnTransforms();
	}

	protected override void GameJoinSession(GameJoinSessionEvent e)
	{
		_currentMapId.Value = MenuManager.Instance.GetInputMapId()-1; // MapId will be in [1..4] but the list start at 0
		UpdateSpawnTransforms();
	}

	// TODO : Think about a better method to change dynamicly the mapId at the end of a party for exemple
	// protected override void GameChangeMap(GameChangeMapEvent e)
	// {
	// 	_currentMapId.Value++;
	// 	InitMap();
	// }
}
