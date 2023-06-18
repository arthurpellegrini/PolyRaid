using System.Net;
using SDD.Events;
using Unity.Netcode;

public class PlayerNetworkHealth : NetworkBehaviour
{
    private int HealthPoint;
    public int GetHealthPoint() => HealthPoint;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        HealthPoint = 100;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateHealthServerRpc(int damage, ulong clientId)
    {
        var clientWithDamaged = NetworkManager.Singleton.ConnectedClients[clientId]
            .PlayerObject.GetComponent<PlayerNetworkHealth>();

        if (clientWithDamaged != null && clientWithDamaged.HealthPoint > 0)
        {
            clientWithDamaged.HealthPoint -= damage;
        }

        // execute method on a client getting punch
        NotifyHealthChangedClientRpc(damage, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        });
    }

    [ClientRpc]
    public void NotifyHealthChangedClientRpc(int damage, ClientRpcParams clientRpcParams = default)
    {
        if (IsOwner) return;
        HealthPoint -= damage;
        EventManager.Instance.Raise(new PlayerHealthChangedEvent() { eHealth = HealthPoint });
    }
}
