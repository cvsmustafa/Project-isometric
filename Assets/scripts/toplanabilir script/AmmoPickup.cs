using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 30; // Bu unsurun ne kadar mermi artt�raca��

    void OnTriggerEnter(Collider other)
    {
        // E�er bu unsur bir oyuncuya temas ederse
        if (other.gameObject.CompareTag("Player"))
        {
            // Oyuncunun automatic scriptini al
            automatic playerAutomatic = other.gameObject.GetComponent<automatic>();

            // Oyuncunun mermi say�s�n� artt�r
            playerAutomatic.AddAmmo(ammoAmount);

            // Bu unsuru yok et
            Destroy(gameObject);
        }
    }
}