using System.Collections;
using System.Collections.Generic;
using SDD.Events;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkHealth : NetworkBehaviour
{
    private NetworkVariable<int> HealthPoint = new NetworkVariable<int>();
    public NetworkVariable<int> GetHealthPoint() => HealthPoint;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        HealthPoint.Value = 100;
    }
    
    private void OnEnable() { HealthPoint.OnValueChanged += HealthChanged; }
    private void OnDisable() { HealthPoint.OnValueChanged -= HealthChanged; }

    private void HealthChanged(int previousValue, int newValue)
    {
        EventManager.Instance.Raise(new PlayerHealthChangedEvent() { eHealth = HealthPoint.Value });
    }

    public void TakeDamage(int damage) { HealthPoint.Value -= damage; }
}
