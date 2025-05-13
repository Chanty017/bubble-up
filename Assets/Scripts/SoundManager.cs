using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private List<AudioClip> soundEffects = new List<AudioClip>();
    private Dictionary<string, AudioClip> soundEffectDict;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        soundEffectDict = new Dictionary<string, AudioClip>();

        foreach (var clip in soundEffects)
        {
            if (clip != null)
            {
                soundEffectDict[clip.name] = clip;
            }
        }
    }

    public void PlaySFX(string soundName, float volume = 1.0f)
    {
        if (soundEffectDict.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} not found!");
        }
    }

    public void AddSoundEffect(AudioClip clip)
    {
        if (clip != null && !soundEffectDict.ContainsKey(clip.name))
        {
            soundEffects.Add(clip);
            soundEffectDict[clip.name] = clip;
        }
    }

    public void RemoveSoundEffect(string soundName)
    {
        if (soundEffectDict.ContainsKey(soundName))
        {
            AudioClip clip = soundEffectDict[soundName];
            soundEffects.Remove(clip);
            soundEffectDict.Remove(soundName);
        }
    }
}
