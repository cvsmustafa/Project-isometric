using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public GameObject healthPickup; // Can arttýran unsur

    void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            float dropChance = Random.Range(0f, 1f);

            if (dropChance < 0.2f)
            {
                Instantiate(healthPickup, transform.position, Quaternion.identity);
            }

            Die();
        }
    }

    void Die()
    {
        GameControlManager.instance.EnemyKilled(); // Düþman öldüðünde GameManager'a bildir
        Destroy(gameObject);
    }
}
