using UnityEngine;

public enum SoundEffect
{
    PlayerHitWall,
    PlayerOpenDoor,
    PlayerFoundMerchant,
    PlayerPurchase,
    PlayerActionFailed
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sound Effects")]
    public AudioClip playerHitWall;
    public AudioClip playerOpenDoor;
    public AudioClip playerFoundMerchant;
    public AudioClip playerPurchase;
    public AudioClip playerActionFailed;

    [Header("Music")]
    public AudioClip backgroundMusic;

    private AudioSource musicSource;

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

        musicSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlaySound(SoundEffect sound)
    {
        AudioClip clip = null;

        switch (sound)
        {
            case SoundEffect.PlayerHitWall: clip = playerHitWall; break;
            case SoundEffect.PlayerOpenDoor: clip = playerOpenDoor; break;
            case SoundEffect.PlayerFoundMerchant: clip = playerFoundMerchant; break;
            case SoundEffect.PlayerPurchase: clip = playerPurchase; break;
            case SoundEffect.PlayerActionFailed: clip = playerActionFailed; break;
        }

        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public void PlayMusic(AudioClip music)
    {
        if (music == null) return;
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}