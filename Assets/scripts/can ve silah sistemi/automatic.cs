using System.Collections;
using UnityEngine;

public class automatic : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public otoweapon gun;
    private InputHandler _input;
    public ParticleSystem muzzleFlash;
    private bool isFiring = false;
    public bool isActive = false; // Silahýn aktif olup olmadýðýný kontrol eden deðiþken

    public int currentBullets; // Current bullets in the magazine
    public int currentCarryBullets; // Current bullets player is carrying

    public BulletPool bulletPool; // Bu silahýn mermi havuzu

    public AudioSource audioSource; // AudioSource bileþeni
    public AudioClip fireSound; // Ateþleme sesi
    public AudioClip reloadSound; // Mermi doldurma sesi

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        currentBullets = gun.maxBullets; // Initialize the magazine
        currentCarryBullets = gun.maxCarryBullets; // Initialize the carry bullets
    }

    void Update()
    {
        if (isActive && UnityEngine.Input.GetButton("Fire1") && !isFiring && currentBullets > 0)
        {
            StartCoroutine(Shoot());
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.R)) // Check if R key is pressed
        {
            Reload();
        }
    }

    void Reload()
    {
        int bulletsNeeded = gun.maxBullets - currentBullets; // Calculate the number of bullets needed to refill the magazine

        if (currentCarryBullets >= bulletsNeeded)
        {
            currentBullets = gun.maxBullets; // Refill the magazine
            currentCarryBullets -= bulletsNeeded; // Decrease the carry bullets by the number of bullets needed
        }
        else if (currentCarryBullets > 0)
        {
            currentBullets += currentCarryBullets; // Add the remaining carry bullets to the magazine
            currentCarryBullets = 0; // Set carry bullets to zero
        }

        audioSource.PlayOneShot(reloadSound); // Mermi doldurma sesini çal
    }

    IEnumerator Shoot()
    {
        muzzleFlash.Play();
        isFiring = true;
        currentBullets--; // Decrease the bullet count

        GameObject bullet = bulletPool.GetBullet(); // Bu silahýn mermi havuzundan bir mermi al
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(GetMouseDirection());
        bullet.SetActive(true);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * gun.bulletSpeed;

        audioSource.PlayOneShot(fireSound); // Ateþleme sesini çal

        StartCoroutine(ReturnBulletToPool(bullet, gun.bulletLifeTime));

        yield return new WaitForSeconds(gun.fireRate);

        isFiring = false;
    }

    IEnumerator ReturnBulletToPool(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bulletPool.ReturnBullet(bullet);
    }

    Vector3 GetMouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(_input.MousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = firePoint.position.y;
            return (target - firePoint.position).normalized;
        }
        return firePoint.forward;
    }

    public void AddAmmo(int amount)
    {
        // Mermi sayýsýný arttýr, ancak maksimum mermi sayýsýný aþmasýna izin verme
        currentCarryBullets = Mathf.Min(currentCarryBullets + amount, gun.maxCarryBullets);
    }
}