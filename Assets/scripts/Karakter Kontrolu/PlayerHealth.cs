using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager sýnýfýný kullanabilmek için bu kütüphaneyi ekledik

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 500f; // Oyuncunun maksimum caný
    public float currentHealth; // Oyuncunun mevcut caný
    public GameObject gameOverScreen; // Game Over ekraný referansý

    void Start()
    {
        // Oyun baþladýðýnda, oyuncunun mevcut canýný maksimum cana eþitle
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        // Caný azalt
        currentHealth -= amount;

        // Caný bittiyse öl
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        // Caný arttýr, ancak maksimum caný aþmasýna izin verme
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    void Die()
    {
        // Oyuncunun ölmesini saðlar
        gameOverScreen.SetActive(true); // Game Over ekranýný göster
        Time.timeScale = 0f; // Oyunu durdur
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Oyunu yeniden baþlat
        SceneManager.LoadScene("MainMenu"); // "MainMenu" sahnesine geçiþ yapar
    }
}