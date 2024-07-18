using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 50f; // Bu unsurun ne kadar can artt�raca��

    void OnTriggerEnter(Collider other)
    {
        // E�er bu unsur bir oyuncuya temas ederse
        if (other.gameObject.CompareTag("Player"))
        {
            // Oyuncunun PlayerHealth scriptini al
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            // Oyuncunun can�n� artt�r
            playerHealth.Heal(healAmount);

            // Bu unsuru yok et
            Destroy(gameObject);
        }
    }
}