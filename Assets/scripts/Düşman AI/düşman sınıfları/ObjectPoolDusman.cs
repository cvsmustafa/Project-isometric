using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolDusman : MonoBehaviour
{
    public static ObjectPoolDusman Instance { get; private set; }

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private int initialPoolSize = 100;

    private Queue<GameObject> bullets;

    private void Awake()
    {
        Instance = this;

        bullets = new Queue<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullets.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (bullets.Count == 0)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullets.Enqueue(bullet);
        }

        return bullets.Dequeue();
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullets.Enqueue(bullet);
    }
}