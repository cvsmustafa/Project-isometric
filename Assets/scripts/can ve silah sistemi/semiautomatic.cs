using System.Collections;
using UnityEngine;

public class semiautomatic : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private InputHandler _input;
    public ParticleSystem muzzleFlash;
    // Gun ScriptableObject referans�
    public semiweapon gun;

    private int bulletCount;
    private float lastFireTime = 0f;
    public bool isActive = false; // Silah�n aktif olup olmad���n� kontrol eden de�i�ken

    public BulletPool bulletPool; // Bu silah�n mermi havuzu

    public AudioSource audioSource; // AudioSource bile�eni
    public AudioClip fireSound; // Ate�leme sesi
    public AudioClip reloadSound; // Mermi doldurma sesi

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        bulletCount = gun.maxBulletCount; // Ba�lang��ta �arj�r� doldur
    }

    void Update()
    {
        if (isActive && UnityEngine.Input.GetButtonDown("Fire1") && bulletCount > 0 && Time.time - lastFireTime >= gun.fireRate)
        {
            Shoot();
            bulletCount--; // Mermi say�s�n� azalt
            lastFireTime = Time.time; // Son ate�leme zaman�n� g�ncelle
        }

        // "R" tu�una bas�ld���nda �arj�r� yeniden doldur
        if (UnityEngine.Input.GetKeyDown(KeyCode.R) && gun.totalBulletCount > 0)
        {
            int bulletsToReload = gun.maxBulletCount - bulletCount;
            if (gun.totalBulletCount >= bulletsToReload)
            {
                bulletCount = gun.maxBulletCount;
                gun.totalBulletCount -= bulletsToReload;
            }
            else
            {
                bulletCount += gun.totalBulletCount;
                gun.totalBulletCount = 0;
            }

            audioSource.PlayOneShot(reloadSound); // Mermi doldurma sesini �al
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        audioSource.PlayOneShot(fireSound); // Ate�leme sesini �al

        GameObject bullet = bulletPool.GetBullet(); // Bu silah�n mermi havuzundan bir mermi al
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(GetMouseDirection());
        bullet.SetActive(true);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * gun.bulletSpeed;

        StartCoroutine(ReturnBulletToPool(bullet, gun.bulletLifeTime));
    }

    IEnumerator ReturnBulletToPool(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bulletPool.ReturnBullet(bullet); // Bu silah�n mermi havuzuna mermiyi geri koy
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
}