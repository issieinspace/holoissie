using Prime31.MessageKit;
using System.Collections;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Range(0,1)]
    public float maxVolume;
    public float volumeIncreasePerSecond;
    public AudioSource chime;

    private AudioSource audioSource;

    void Start()
    {
        MessageKit.addObserver(MessageType.OnGameOver, OnGameOver);
    }

    void OnGameOver()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        chime.Play();
        StartCoroutine(FadeInAudio());
    }

    IEnumerator FadeInAudio()
    {
        bool finished = false;
        while (!finished)
        {
            audioSource.volume += volumeIncreasePerSecond * Time.deltaTime;

            if (audioSource.volume >= maxVolume)
            {
                audioSource.volume = maxVolume;
                finished = true;
            }
            yield return null;
        }
    }
}

