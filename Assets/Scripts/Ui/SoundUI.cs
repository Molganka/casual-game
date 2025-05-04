using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUI : MonoBehaviour
{
    public static SoundUI Instance;

    [SerializeField] private Sound[] _audioClips;
    private AudioSource _audioSource => GetComponent<AudioSource>();

    public enum AudioClipsEnum
    {
        Open,
        Close,
        Item,
        Coin,
        Block
    }

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(AudioClipsEnum clip)
    {
        if(GameData.OnUISound)
            _audioSource.PlayOneShot(_audioClips[(int)clip].audio, _audioClips[(int)clip].volume);
    }

    [Serializable]
    public struct Sound
    {
        public AudioClip audio;
        public float volume;
    }
}
