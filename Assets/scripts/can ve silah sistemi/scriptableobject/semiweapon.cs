using UnityEngine;

[CreateAssetMenu(fileName = "Yariotomatik", menuName = "Weapon/Yarý otomatik", order = 1)]
public class semiweapon : ScriptableObject
{
    public float bulletSpeed = 20f;
    public float bulletLifeTime = 2f;
    public int maxBulletCount = 10;
    public int totalBulletCount = 60;
    public float fireRate = 0.5f;
}