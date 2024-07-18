using System.Collections;
using UnityEngine;

public class AutomaticShooter : MonoBehaviour
{
    public BulletPool bulletPool;
    public BulletSpawner bulletSpawner;
    public BulletMover bulletMover;
    public BulletReturner bulletReturner;
    public InputHandler inputHandler;

    private bool isFiring = false;

    void Update()
    {
        if (UnityEngine.Input.GetButton("Fire1") && !isFiring)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        isFiring = true;

        GameObject bullet = bulletPool.GetBullet();
        bulletSpawner.Spawn(bullet);
        bulletMover.Move(bullet);

        StartCoroutine(bulletReturner.ReturnBulletToPool(bullet));

        yield return new WaitForSeconds(bulletReturner.fireRate);

        isFiring = false;
    }
}

public class BulletSpawner
{
    public Transform firePoint;
    public InputHandler inputHandler;

    public void Spawn(GameObject bullet)
    {
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(GetMouseDirection());
        bullet.SetActive(true);
    }

    Vector3 GetMouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(inputHandler.MousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = firePoint.position.y;
            return (target - firePoint.position).normalized;
        }
        return firePoint.forward;
    }
}

public class BulletMover
{
    public float bulletSpeed = 20f;

    public void Move(GameObject bullet)
    {
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
    }
}

public class BulletReturner
{
    public BulletPool bulletPool;
    public float bulletLifeTime = 2f;
    public float fireRate = 0.1f; // 10 bullets per second

    public IEnumerator ReturnBulletToPool(GameObject bullet)
    {
        yield return new WaitForSeconds(bulletLifeTime);
        bulletPool.ReturnBullet(bullet);
    }
}