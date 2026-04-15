using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of all available sound effects in the game.
/// </summary>
public enum SoundEffects
{
    land,
    swap,
    resolve,
    upgrade,
    powerup,
    score
}

/// <summary>
/// Manages background music and sound effect playback.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    #region Variables

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] sounds;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource soundEffects;

    #endregion
    protected override void Init()
    {
        soundEffects = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        music.Play();
    }

    public void PauseMusic(bool pause)
    {
        if(pause)
            music.Pause();

        else
            music.UnPause();
    }

    /// <summary>
    /// Plays a sound effect once based on the given enum type.
    /// </summary>
    public void PlaySound(SoundEffects effect)
    {
        soundEffects.PlayOneShot(sounds[ (int) effect]);
    }

    /// <summary>
    /// Waits for a few seconds before playing a sound.
    /// </summary>
    public IEnumerator PlayDelayedSound(SoundEffects effect, float t)
    {
        yield return new WaitForSeconds(t);
        PlaySound(effect);
    }
}
