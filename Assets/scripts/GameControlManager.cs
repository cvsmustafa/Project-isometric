using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControlManager : MonoBehaviour
{
    public static GameControlManager instance;
    public GameObject missionSuccessfulScreen; // Mission Successful ekraný referansý
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
        // Baþlangýçta düþman sayýsýný bul
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
        missionSuccessfulScreen.SetActive(true); // Mission Successful ekranýný göster
        Time.timeScale = 0f; // Oyunu durdur
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Oyunu yeniden baþlat
        SceneManager.LoadScene("MainMenu"); // "MainMenu" sahnesine geçiþ yapar
    }

    /*public void Continue()
    {
        Time.timeScale = 1f; // Oyunu yeniden baþlat
        // Bir sonraki sahneye geçiþ yapabilir
        // "SceneManager.LoadScene("NextLevel");"
    }*/
}
