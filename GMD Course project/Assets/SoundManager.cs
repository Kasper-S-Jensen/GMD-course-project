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

    /*private static SoundManager instance;
    public AudioClip playerAttackSoundClip;
    public AudioClip playerJumpSoundClip;

    public AudioClip gameBackGroundMusicClip;
    private AudioSource gameBackGroundMusic;
    private AudioSource playerAttackSound;
    private AudioSource playerJumpSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            gameBackGroundMusic = new AudioSource();
            playerJumpSound = new AudioSource();
            playerJumpSound = new AudioSource();
            DontDestroyOnLoad(gameObject);
            return;
        }

        if (instance == this)
        {
            return;
        }


        Destroy(gameObject);
    }

    private void Start()
    {
        gameBackGroundMusic.clip = gameBackGroundMusicClip;
        gameBackGroundMusic.PlayOneShot(gameBackGroundMusicClip);
    }
*/
    public void PlayerJump(Component sender, object data)
    {
        // playerJumpSound.clip = playerJumpSoundClip;
        // playerJumpSound.PlayOneShot(playerJumpSoundClip);
        PlayAudioClip("Jump");
    }

    public void PlayerAttack(Component sender, object data)
    {
        PlayAudioClip("PlayerAttack");
        //  playerAttackSound.clip = playerAttackSoundClip;
        //  playerAttackSound.PlayOneShot(playerAttackSoundClip);
    }

    [Serializable]
    public struct AudioInfo
    {
        public string clipName;
        public AudioClip clip;
    }
}