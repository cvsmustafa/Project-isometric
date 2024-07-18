using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private AudioSource menuMusic;
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
        menuMusic.Stop();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}