using UnityEngine;

public class GameMusicController : MonoBehaviour
{
    private AudioSource gameMusic;

    void Start()
    {
        gameMusic.Play(); // Oyun baþladýðýnda müziði çalar
    }
}