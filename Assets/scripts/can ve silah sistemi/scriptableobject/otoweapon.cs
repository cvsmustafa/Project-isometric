using UnityEngine;

[CreateAssetMenu(fileName = "otomatik", menuName = "Weapon/otomatik", order = 1)]
public class otoweapon : ScriptableObject
{
    public float bulletSpeed = 20f;
    public float bulletLifeTime = 2f;
    public float fireRate = 0.1f; // 10 bullets per second
    public int maxBullets = 30; // Maximum bullets in a magazine
    public int maxCarryBullets = 180; // Maximum bullets player can carry
}