using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlManager : MonoBehaviour
{
    public static GameControlManager instance;
    public GameObject missionSuccessfulScreen; // Mission Successful ekran� referans�
    private int enemyCount;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Ba�lang��ta d��man say�s�n� bul
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void EnemyKilled()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            MissionSuccessful();
        }
    }

    void MissionSuccessful()
    {
        missionSuccessfulScreen.SetActive(true); // Mission Successful ekran�n� g�ster
        Time.timeScale = 0f; // Oyunu durdur
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Oyunu yeniden ba�lat
        SceneManager.LoadScene("MainMenu"); // "MainMenu" sahnesine ge�i� yapar
    }

    /*public void Continue()
    {
        Time.timeScale = 1f; // Oyunu yeniden ba�lat
        // Bir sonraki sahneye ge�i� yapabilir
        // "SceneManager.LoadScene("NextLevel");"
    }*/
}
