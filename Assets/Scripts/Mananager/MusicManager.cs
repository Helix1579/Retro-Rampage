using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0.05f;
    }

    public void PlayMenuMusic()
    {
        if (audioSource.clip != menuMusic)
        {
            audioSource.clip = menuMusic;
            audioSource.Play();
        }
    }

    public void PlayGameplayMusic()
    {
        if (audioSource.clip != gameplayMusic)
        {
            audioSource.clip = gameplayMusic;
            audioSource.Play();
        }
    }
}
