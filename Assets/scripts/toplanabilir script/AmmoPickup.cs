using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 30; // Bu unsurun ne kadar mermi arttýracaðý

    void OnTriggerEnter(Collider other)
    {
        // Eðer bu unsur bir oyuncuya temas ederse
        if (other.gameObject.CompareTag("Player"))
        {
            // Oyuncunun automatic scriptini al
            automatic playerAutomatic = other.gameObject.GetComponent<automatic>();

            // Oyuncunun mermi sayýsýný arttýr
            playerAutomatic.AddAmmo(ammoAmount);

            // Bu unsuru yok et
            Destroy(gameObject);
        }
    }
}