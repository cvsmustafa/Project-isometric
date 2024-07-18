using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager s�n�f�n� kullanabilmek i�in bu k�t�phaneyi ekledik

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 500f; // Oyuncunun maksimum can�
    public float currentHealth; // Oyuncunun mevcut can�
    public GameObject gameOverScreen; // Game Over ekran� referans�

    void Start()
    {
        // Oyun ba�lad���nda, oyuncunun mevcut can�n� maksimum cana e�itle
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        // Can� azalt
        currentHealth -= amount;

        // Can� bittiyse �l
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        // Can� artt�r, ancak maksimum can� a�mas�na izin verme
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    void Die()
    {
        // Oyuncunun �lmesini sa�lar
        gameOverScreen.SetActive(true); // Game Over ekran�n� g�ster
        Time.timeScale = 0f; // Oyunu durdur
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Oyunu yeniden ba�lat
        SceneManager.LoadScene("MainMenu"); // "MainMenu" sahnesine ge�i� yapar
    }
}