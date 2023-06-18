using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Transform cam;

    [SerializeField] private bool rapidFire = false;
    [SerializeField] private float range = 50f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 5f;
    private WaitForSeconds rapidFireWait;

    [SerializeField] private int maxAmmo = 30;
    private int currentAmmo;
    
    [SerializeField] private float reloadTime;
    private WaitForSeconds reloadWait;
    
    private void Awake()
    {
        cam = Camera.main.transform;
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
    }

    public void Shoot()
    {
        currentAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, range))
        {
            Debug.Log(hit.collider);
            // if (hit.collider.GetComponent<Damageable>() != null)
            // {
            //     hit.collider.GetComponent<Damageable>().TakeDamage(damage, hit.point, hit.normal);
            // }
        }
    }

    public IEnumerator RapidFire()
    {
        if (CanShoot())
        {
            Shoot();
            if (rapidFire)
            {
                while (CanShoot())
                {
                    yield return rapidFireWait;
                    Shoot();
                }
                StartCoroutine(Reload());
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

        print("Reloading...");
        yield return reloadWait;
        currentAmmo = maxAmmo;
        print("Finished reloading");
    }

    bool CanShoot()
    {
        return currentAmmo > 0;
    }
}
