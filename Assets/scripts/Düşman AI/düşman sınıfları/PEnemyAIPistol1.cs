using UnityEngine;

public class PEnemyAIPistol1 : EnemyAI
{
    public GameObject bulletPrefab;
    public Transform gunBarrelEnd;
    public ParticleSystem muzzleFlash;

    public float fireRate = 3; // Her saniyede ateþ edilen mermi sayýsý
    private float fireTimer = 0; // Ateþ etme zamanlayýcýsý
    public float bulletSpeed = 50;

    public AudioSource audioSource; // AudioSource bileþeni
    public AudioClip fireSound; // Ateþleme sesi

    protected override void Update()
    {
        base.Update();

        fireTimer += Time.deltaTime;

        var dir = (player.transform.position - transform.position).normalized;
        bool IsItOpen = Physics.Linecast(transform.position + Vector3.up, player.transform.position + Vector3.up, obstacleMask);

        if (state == State.Follow && fireTimer >= 1 / fireRate && !IsItOpen)
        {
            Fire();
            fireTimer = 0;
        }
    }

    void Fire()
    {
        if (audioSource == null || fireSound == null)
        {
            Debug.LogError("AudioSource or fireSound is not assigned.");
            return;
        }

        // Mermi nesnesini havuzdan al
        GameObject bullet = ObjectPoolDusman.Instance.GetBullet();
        bullet.transform.position = gunBarrelEnd.position;
        bullet.transform.rotation = gunBarrelEnd.rotation;

        bullet.SetActive(true);
        muzzleFlash.Play();

        audioSource.PlayOneShot(fireSound); // Ateþleme sesini çal

        // Mermiyi silahýn ucundan fýrlat
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = gunBarrelEnd.forward * bulletSpeed;
    }
}