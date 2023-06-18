using System.Collections;
using SDD.Events;
using UnityEngine;
using Event = SDD.Events.Event;

public class Weapon : MonoBehaviour
{
    private Transform cam;
    
    private bool canReload = true;
    [SerializeField] private float range = 50f;
    [SerializeField] private int damage = 10;
    [SerializeField] private int maxAmmo = 30;
    private int currentAmmo; // TODO: SETTER CURRENT AMMO -> AVEC LEVEE EVENEMENT EN +
    [SerializeField] private float reloadTime;
    private WaitForSeconds reloadWait;
    private int score = 0;
    
    
    private void Awake()
    {
        cam = Camera.main.transform;
        reloadWait = new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        EventManager.Instance.Raise(new PlayerMagChangedEvent() { eMag = currentAmmo });
        EventManager.Instance.Raise(new PlayerScoreChangedEvent() { eScore = score });
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            currentAmmo--;
            EventManager.Instance.Raise(new PlayerMagChangedEvent() { eMag = currentAmmo });
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, range))
            {
                Debug.Log(currentAmmo + ", " + hit.collider);
                if (hit.collider.GetComponent<PlayerNetworkHealth>() != null)
                {
                    Debug.Log("Hit Player !!" + hit.collider);
                    score += 100;
                    EventManager.Instance.Raise(new PlayerScoreChangedEvent() { eScore = score });
                    hit.collider.GetComponent<PlayerNetworkHealth>().TakeDamage(damage);
                }

                // {
                //     hit.collider.GetComponent<Damageable>().TakeDamage(damage, hit.point, hit.normal);
                // }
            }
        }
        else
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        if (currentAmmo == maxAmmo)
        {
            yield return null;
        }
        else if (canReload)
        {
            canReload = false;
            print("Reloading...");
            yield return reloadWait;
            currentAmmo = maxAmmo;
            EventManager.Instance.Raise(new PlayerMagChangedEvent() { eMag = currentAmmo });
            print("Finished reloading");
            canReload = true;
        }
        else yield return null;

    }

    bool CanShoot()
    {
        return currentAmmo > 0;
    }
}
