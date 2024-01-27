using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public List<AudioClip> clips;
    [Range(0f, 1f)]
    public float volume = 1f;
    public bool loop = false;
    [Range(0f, 3f)]
    public float pitchRange = 0.1f; // Adjust the pitch range as needed
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<Sound> sounds;
    public int maxSimultaneousSounds = 5; // Adjust the number of simultaneous sounds as needed

    private List<AudioSource> audioSources;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSources = new List<AudioSource>();
        for (int i = 0; i < maxSimultaneousSounds; i++)
        {
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(newAudioSource);
        }

        foreach (Sound sound in sounds)
        {
            sound.name = sound.clips[0].name; // Set the name to the first clip's name for easier reference
        }
    }

    public void PlaySound(string clipName)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == clipName);

        if (soundToPlay != null)
        {
            AudioClip clipToPlay = soundToPlay.clips[Random.Range(0, soundToPlay.clips.Count)];

            AudioSource freeAudioSource = GetFreeAudioSource();
            if (freeAudioSource != null)
            {
                freeAudioSource.clip = clipToPlay;
                freeAudioSource.volume = soundToPlay.volume;
                freeAudioSource.loop = soundToPlay.loop;

                if (soundToPlay.pitchRange > 0)
                {
                    float randomPitch = 1f + Random.Range(-soundToPlay.pitchRange, soundToPlay.pitchRange);
                    freeAudioSource.pitch = randomPitch;
                }

                freeAudioSource.Play();
            }
            else
            {
                Debug.LogWarning("All audio sources are busy. Unable to play sound: " + clipName);
            }
        }
        else
        {
            Debug.LogError("Sound not found: " + clipName);
        }
    }

    private AudioSource GetFreeAudioSource()
    {
        return audioSources.Find(source => !source.isPlaying);
    }
}
