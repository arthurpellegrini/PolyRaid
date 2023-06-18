using System.Collections;
using SDD.Events;
using UnityEngine;
using Event = SDD.Events.Event;

public class Weapon : MonoBehaviour
{
    private Transform cam;

    [SerializeField] private bool rapidFire = false;
    private bool canReload = true;
    [SerializeField] private float range = 50f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float fireRate = 5f;
    private WaitForSeconds rapidFireWait;

    [SerializeField] private int maxAmmo = 30;
    private int currentAmmo; // TODO: SETTER CURRENT AMMO -> AVEC LEVEE EVENEMENT EN +
    
    [SerializeField] private float reloadTime;
    private WaitForSeconds reloadWait;
    
    private void Awake()
    {
        cam = Camera.main.transform;
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        EventManager.Instance.Raise(new PlayerMagChangedEvent() { eMag = currentAmmo });
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
