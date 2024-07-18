using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 50f; // Bu unsurun ne kadar can arttýracaðý

    void OnTriggerEnter(Collider other)
    {
        // Eðer bu unsur bir oyuncuya temas ederse
        if (other.gameObject.CompareTag("Player"))
        {
            // Oyuncunun PlayerHealth scriptini al
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            // Oyuncunun canýný arttýr
            playerHealth.Heal(healAmount);

            // Bu unsuru yok et
            Destroy(gameObject);
        }
    }
}