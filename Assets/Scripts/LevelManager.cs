using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
	private NetworkObject _currentMapNo;
	private List<Transform> _spawnPoints;
	private int _randomSpawnPoint;

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
	
	// Déclarez une variable synchronisée pour stocker le spawn point
	private NetworkVariable<Vector3> _syncedSpawnPoint = new NetworkVariable<Vector3>(Vector3.zero);
	public Vector3 GetSpawnPoint() => _syncedSpawnPoint.Value;

	[ServerRpc]
	public void ResponseSpawnPointToServerRpc(ulong clientId)
	{
		Vector3 spawnPoint = GetRandomSpawnPoint();
		// Envoyer le SpawnPoint au client spécifié
		ResponseSpawnPointToClientRpc(clientId, spawnPoint);
	}

	[ClientRpc]
	private void ResponseSpawnPointToClientRpc(ulong clientId, Vector3 spawnPoint)
	{
		// Vérifier si le client actuel correspond au client spécifié
		if (GetComponent<NetworkObject>().OwnerClientId == clientId)
		{
			// Définir la position du joueur sur le SpawnPoint reçu du serveur
			transform.position = spawnPoint;
		}
	}

	// [ServerRpc]
	// public void RequestSpawnPointFromServerRpc()
	// {
	// 	// Vérifiez si la liste des points de spawn est vide
	// 	if (_spawnPoints.Count == 0)
	// 	{
	// 		Debug.LogError("La liste des points de spawn est vide !");
	// 		_syncedSpawnPoint.Value = Vector3.zero; // Réglez la valeur sur une valeur par défaut appropriée
	// 	}
	// 	else
	// 	{
	// 		Vector3 spawn = _spawnPoints[_randomSpawnPoint++ % _spawnPoints.Count].position;
	// 		Debug.Log(_randomSpawnPoint.ToString() + ";" + spawn.ToString());
	//
	// 		// Affectez la valeur du spawn point à la variable synchronisée
	// 		_syncedSpawnPoint.Value = spawn;
	// 	}
	//
	// 	// Appeler la fonction RPC pour notifier le client
	// 	ResponseSpawnPointToClientRpc();
	// }
	//
	// [ClientRpc]
	// public void ResponseSpawnPointToClientRpc()
	// {
	// 	// Accédez à la valeur de la variable synchronisée pour obtenir le spawn point
	// 	Vector3 spawnPoint = _syncedSpawnPoint.Value;
	//
	// 	// Traitez la réponse côté client
	// 	Debug.Log("Spawn point reçu du serveur : " + spawnPoint);
	// }
	
	public Vector3 GetRandomSpawnPoint()
	{
		if (_spawnPoints.Count == 0)
		{
			Debug.LogError("La liste des points de spawn est vide !");
			return new Vector3(0, 0, 0);
		}
	
		Vector3 spawn = _spawnPoints[_randomSpawnPoint++ % _spawnPoints.Count].position;
		Debug.Log(_randomSpawnPoint.ToString() + ";" + spawn.ToString());
		return spawn;
	}

	private void InitMap()
	{
		_mapState = 0;

		if (IsHost)
		{
			// _mapState = (MapState)Random.Range(0, 2); // Synchronize with server that radom each new session
			_currentMapGo = Instantiate(allMapsPrefab[(int)_mapState], Vector3.zero, Quaternion.identity);
			_currentMapNo = _currentMapGo.GetComponent<NetworkObject>();
			_currentMapNo.Spawn();
		}
		_spawnPoints = _testPrefab.SpawnPoints;
	}

	private void DestroyMap()
	{
		if(_currentMapNo) _currentMapNo.Despawn();
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