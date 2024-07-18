using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageAmount = 10;
    private bool justFired = true;
    private Collider bulletCollider;

    void OnEnable()
    {
        justFired = true;
        if (bulletCollider == null)
        {
            bulletCollider = GetComponent<Collider>();
        }
        bulletCollider.enabled = false;
        StartCoroutine(EnableCollision());
    }

    IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(0.05f);
        bulletCollider.enabled = true;
        justFired = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (justFired) return;

        var enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damageAmount);
        }
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}