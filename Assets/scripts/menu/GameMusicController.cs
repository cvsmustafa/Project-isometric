using UnityEngine;

public class GameMusicController : MonoBehaviour
{
    private AudioSource gameMusic;

    void Start()
    {
        gameMusic.Play(); // Oyun ba�lad���nda m�zi�i �alar
    }
}