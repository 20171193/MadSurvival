using System.Collections.Generic;
using UnityEngine;
using Jc;
using System;
using System.Linq;
using UnityEngine.Audio;

namespace Jc
{
    [Serializable]
    public enum UISFXType
    {
        NormalButton = 0,
        StartButton
    }
}
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] 
    private AudioSource bgmSource;
    [SerializeField]
    private AudioClip[] bgmClips;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private AudioMixer audioMixer;
    public AudioMixer AudioMixer { get { return audioMixer; }  }

    [Header("기본버튼, 시작버튼")]
    [SerializeField]
    private AudioClip[] sfxClips;

    public float BGMVolme { get { return bgmSource.volume; } set { bgmSource.volume = value; } }
    public float SFXVolme { get { return sfxSource.volume; } set { sfxSource.volume = value; } }

    public void PlayBGM(int index)
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
        bgmSource.clip = bgmClips[index];
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource == null) return;
        if (bgmSource.isPlaying == false)
            return;

        bgmSource.Stop();
    }

    public void PlaySFX(int index)
    {
        if (index >= sfxClips.Length) return;

        sfxSource.clip = sfxClips[index];
        sfxSource.Play();
    }

    public void StopSFX()
    {
        if (sfxSource.isPlaying == false)
            return;

        sfxSource.Stop();
    }
}