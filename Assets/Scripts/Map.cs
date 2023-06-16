using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Map : NetworkBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    public List<Transform> SpawnPoints => _spawnPoints;
}