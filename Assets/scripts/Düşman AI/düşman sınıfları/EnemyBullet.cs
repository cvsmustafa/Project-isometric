using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(10);
        }
        ObjectPoolDusman.Instance.ReturnBullet(gameObject);
    }

}