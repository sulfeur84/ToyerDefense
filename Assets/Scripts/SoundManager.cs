using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource stressedMusic;
    [SerializeField] private AudioSource sfxSource;

    [Space]
    [SerializeField] private List<AudioClip> sfxClips;

    public static Action<bool> PlayStressedMusicA;
    public static Action<string> PlaySfxA;

    private void Awake()
    {
        PlayStressedMusicA = PlayStressedMusic;
        PlaySfxA = PlaySfx;
    }

    private void PlaySfx(string name)
    {
        foreach (AudioClip audioClip in sfxClips)
        {
            if (audioClip.name == name)
            {
                sfxSource.PlayOneShot(audioClip);
                return;
            }
        }
    }

    private void PlayStressedMusic(bool b) //if flag captured then b == true
    {
        music.mute = b;
        stressedMusic.mute = !b;
    }
}
