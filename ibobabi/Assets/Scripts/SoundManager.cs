using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Sound
{
    public string soundname;
    public List<AudioClip> clips;
    [Range(0f, 1f)]
    public float volume = 1f;
    public bool loop = false;
    [Range(0f, 3f)]
    public float pitchRange = 0.1f; // Adjust the pitch range as needed
    public float pitchOffset = 0f;
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
    }

    public void PlaySound(string clipName, bool preventDuplicate = false)
    {
        Sound soundToPlay = null;
        for (int i = 0; i < sounds.Count; i++)
        {
            if (clipName == sounds[i].soundname)
            {
                soundToPlay = sounds[i];
                break;
            }
        }

        if(preventDuplicate)
        {
            if (IsSoundPlaying(clipName))
                return;
        }

        if (soundToPlay != null)
        {
            AudioClip clipToPlay = soundToPlay.clips[Random.Range(0, soundToPlay.clips.Count)];

            AudioSource freeAudioSource = GetFreeAudioSource();
            if (freeAudioSource != null)
            {
                freeAudioSource.clip = clipToPlay;
                freeAudioSource.volume = soundToPlay.volume;
                freeAudioSource.loop = soundToPlay.loop;
                freeAudioSource.pitch = 1f;

                if (soundToPlay.pitchRange > 0)
                {
                    float randomPitch = 1f + Random.Range(-soundToPlay.pitchRange, soundToPlay.pitchRange);
                    freeAudioSource.pitch = randomPitch;
                }
                freeAudioSource.pitch += soundToPlay.pitchOffset;

                freeAudioSource.Play();

                if (!soundToPlay.loop)
                {
                    // If not looping, schedule to stop the sound after its duration
                    StartCoroutine(StopSoundAfterDuration(freeAudioSource, clipToPlay.length));
                }
            }
        }
        else
        {
            Debug.LogError("Sound not found: " + clipName);
        }
    }

    public bool IsSoundPlaying(string soundName)
    {
        Sound soundToCheck = null;
        for (int i = 0; i < sounds.Count; i++)
        {
            if (soundName == sounds[i].soundname)
            {
                soundToCheck = sounds[i];
                break;
            }
        }

        if (soundToCheck == null)
            return false;

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].isPlaying && soundToCheck.soundname == soundName)
                return true;
        }

        return false;
    }

    public AudioSource GetAudioSourcePlayingSound(string soundName)
    {
        Sound soundToCheck = null;
        for (int i = 0; i < sounds.Count; i++)
        {
            if (soundName == sounds[i].soundname)
            {
                soundToCheck = sounds[i];
                break;
            }
        }

        if (soundToCheck == null)
            return null;

        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].isPlaying)
            {
                for(int s = 0; s < soundToCheck.clips.Count; s++)
                {
                    if(soundToCheck.clips[s] == audioSources[i].clip)
                    {
                        return audioSources[i];
                    }
                }
            }
        }

        return null;
    }

    public void StopSound(string soundName)
    {
        AudioSource audioSourceToStop = GetAudioSourcePlayingSound(soundName);

        if (audioSourceToStop != null)
            audioSourceToStop.Stop();
    }
    public void StopAllSounds()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    private AudioSource GetFreeAudioSource()
    {
        return audioSources.Find(source => !source.isPlaying);
    }

    private System.Collections.IEnumerator StopSoundAfterDuration(AudioSource audioSource, float duration)
    {
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }
}
