using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private const string VolumeKey = "Volume";

    [SerializeField] private List<AudioInfo> audioClips;
    [SerializeField] private AudioMixerGroup mixerGroup;

    private List<AudioSource> audioSources;

    private Dictionary<string, AudioClip> clipDictionary;
    private float playerprefVolume;

    private void Awake()
    {
        playerprefVolume = PlayerPrefs.GetFloat(VolumeKey);
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
            source.outputAudioMixerGroup = mixerGroup;
            audioSources.Add(source);
        }


        PlayAudioClip("Background");
    }

    private void Start()
    {
        Debug.Log(playerprefVolume);
        mixerGroup.audioMixer.SetFloat(VolumeKey, playerprefVolume);
        // Save volume level in PlayerPrefs
        // SaveToPlayerPrefs(PlayerPrefs.GetFloat(VolumeKey));
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

    private void StopAudioClip(string clipName)
    {
        // Find the index of the clip in the array using its name
        var clipIndex = -1;
        for (var i = 0; i < audioClips.Count; i++)
        {
            if (audioClips[i].clipName == clipName)
            {
                clipIndex = i;
                break;
            }
        }

        // If the clip index is valid, stop the audio source
        if (clipIndex >= 0)
        {
            audioSources[clipIndex].Stop();
        }
        else
        {
            Debug.LogError("Clip not found: " + clipName);
        }
    }

    public void PlayerJump(Component sender, object data)
    {
        PlayAudioClip("Jump");
    }

    public void PlayerAttack(Component sender, object data)
    {
        StopAudioClip("PlayerAttack");
        PlayAudioClip("PlayerAttack");
    }

    public void GameWon(Component sender, object data)
    {
        StopAudioClip("Background");
        PlayAudioClip("Trumpets");
        PlayAudioClip("CrowdCheer");
    }

    public void OnAdjustVolume(Component sender, object data)
    {
        Debug.Log("reaching");
        if (data is not float amount)
        {
            return;
        }

        mixerGroup.audioMixer.SetFloat(VolumeKey, amount);
        // Save volume level in PlayerPrefs
        SaveToPlayerPrefs(amount);
    }

    private void SaveToPlayerPrefs(float amount)
    {
        PlayerPrefs.SetFloat(VolumeKey, amount);
        PlayerPrefs.Save();
    }

    [Serializable]
    public struct AudioInfo
    {
        public string clipName;
        public AudioClip clip;
    }
}