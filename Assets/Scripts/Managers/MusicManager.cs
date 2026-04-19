using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource menuSourceA;
    public AudioSource menuSourceB;
    public AudioSource gameplaySource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMenuMusic()
    {
        gameplaySource.Stop();
        
        if (!menuSourceA.isPlaying) menuSourceA.Play();
        if (!menuSourceB.isPlaying) menuSourceB.Play();
    }

    public void PlayGameplayMusic()
    {
        menuSourceA.Stop();
        menuSourceB.Stop();
        
        if (!gameplaySource.isPlaying) gameplaySource.Play();
    }
}