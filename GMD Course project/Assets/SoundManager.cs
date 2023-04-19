using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioInfo> audioClips;
    private List<AudioSource> audioSources;

    private Dictionary<string, AudioClip> clipDictionary;

    private void Awake()
    {
        // Build dictionary from the audioClips list
        clipDictionary = new Dictionary<string, AudioClip>();
        foreach (var info in audioClips)
        {
            if (clipDictionary.ContainsKey(info.clipName))
            {
                Debug.LogWarning($"Duplicate clip name: {info.clipName}");
            }
            else
            {
                clipDictionary.Add(info.clipName, info.clip);
            }
        }

        // Create AudioSources for each clip
        audioSources = new List<AudioSource>();
        foreach (var info in audioClips)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = info.clip;
            audioSources.Add(source);
        }

        PlayAudioClip("Background");
    }

    private void PlayAudioClip(string clipName)
    {
        if (!clipDictionary.TryGetValue(clipName, out var clip))
        {
            Debug.LogError($"Audio clip '{clipName}' not found.");
            return;
        }

        // Find an available AudioSource to play the clip
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.Play();
                return;
            }
        }

        // If no available AudioSource is found, log a warning
        Debug.LogWarning("No available AudioSources to play clip.");
    }

    public void PlayerJump(Component sender, object data)
    {
        PlayAudioClip("Jump");
    }

    public void PlayerAttack(Component sender, object data)
    {
        PlayAudioClip("PlayerAttack");
    }

    [Serializable]
    public struct AudioInfo
    {
        public string clipName;
        public AudioClip clip;
    }
}