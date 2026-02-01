using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField]
    private AudioClip defaultBackgroundMusic;

    [SerializeField]
    private AudioClip resultsBackgroundMusic;

    [SerializeField]
    private float musicVolume = 0.8f;

    [SerializeField]
    private float musicVolumeResults = 0.6f;

    [SerializeField]
    private float initialBackgroundMusicfadeInDuration = 3f;

    [SerializeField]
    private float backgroundMusicFadeTime = 0.25f;


    private float defaultBackgroundMusicTimestamp = 0f;

    private void Start()
    {
        ChangeMusicToDefault(initialBackgroundMusicfadeInDuration);
    }

    private void PlayMusic(AudioClip music, float startTimestamp = 0f)
    {
        musicSource.clip = music;
        musicSource.time = startTimestamp;
        musicSource.Play();
    }

    public void StartMusic()
    {
        if (musicSource.clip)
            musicSource.Play();
    }

    public void PlayEffect(AudioClip effect, float volumeScale = 1f)
    {
        if (effect != null)
        {
            sfxSource.pitch = 1;
            sfxSource.PlayOneShot(effect, volumeScale);
        }

    }


    public void PlayEffect(AudioClip effect, float pitchMin, float pitchMax)
    {
        if (effect != null)
        {
            sfxSource.pitch = Random.Range(pitchMin, pitchMax);
            sfxSource.PlayOneShot(effect);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void StopMusic(float timeToStop)
    {
        StartCoroutine(AudioFadeOut(timeToStop));
    }

    public void ChangeMusicToDefault()
    {
        ChangeMusic(defaultBackgroundMusic, backgroundMusicFadeTime);
    }

    public void ChangeMusicToDefault(float fadeTime)
    {
        ChangeMusic(defaultBackgroundMusic, fadeTime);
    }

    public void ChangeMusicToResults(float fadeTime)
    {
        musicVolume = musicVolumeResults;
        ChangeMusic(resultsBackgroundMusic, fadeTime);
    }

    public void ChangeMusic(AudioClip newClip)
    {
        ChangeMusic(newClip, backgroundMusicFadeTime);
    }

    public void ChangeMusic(AudioClip newClip, float timeToChange)
    {
        ChangeMusic(newClip, timeToChange / 2, timeToChange / 2);
    }

    public void ChangeMusic(AudioClip newClip, float fadeOutDuration, float fadeInDuration)
    {
        if (musicSource.clip == newClip) return;

        float startTimestamp = 0f;

        if (musicSource.clip == defaultBackgroundMusic)
        {
            defaultBackgroundMusicTimestamp = musicSource.time;
        }

        if (newClip == defaultBackgroundMusic)
        {
            startTimestamp = defaultBackgroundMusicTimestamp;
        }

        if (musicSource.isPlaying)
        {
            StartCoroutine(CrossFade(newClip, fadeOutDuration, fadeInDuration, startTimestamp));
        }
        else
        {
            StartCoroutine(AudioFadeIn(newClip, fadeInDuration, startTimestamp));
        }
    }

    IEnumerator AudioFadeIn(AudioClip musicClip, float fadeDuration, float startTimestamp = 0f)
    {

        PlayMusic(musicClip, startTimestamp);
        float timeElapsed = 0;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;

            musicSource.volume = Mathf.Lerp(0, musicVolume, timeElapsed / fadeDuration);
            yield return null;
        }
    }

    IEnumerator AudioFadeOut(float fadeDuration)
    {
        float timeElapsed = 0;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;

            musicSource.volume = Mathf.Lerp(musicVolume, 0, timeElapsed / fadeDuration);
            yield return null;
        }

    }

    IEnumerator CrossFade(AudioClip newClip, float fadeOutDuration, float fadeInDuration, float startTimestamp = 0f)
    {
        float timeElapsed = 0;

        while (timeElapsed < fadeOutDuration)
        {
            timeElapsed += Time.deltaTime;

            musicSource.volume = Mathf.Lerp(musicVolume, 0, timeElapsed / fadeOutDuration);
            yield return null;
        }

        timeElapsed = 0;
        PlayMusic(newClip, startTimestamp);

        while (timeElapsed < fadeInDuration)
        {
            timeElapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, musicVolume, timeElapsed / fadeInDuration);
            yield return null;
        }
    }
}