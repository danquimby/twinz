using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private AudioSource audioOpen;
    [SerializeField] private AudioMixerSnapshot InGame;
    [SerializeField] private AudioMixerSnapshot InMenu;

    public void TransitionToMenu()
    {
        Debug.Log("ww");
        InMenu.TransitionTo(0.5f);
    }

    public void TransitionToGame()
    {
        InGame.TransitionTo(0.5f);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"> -80 mute 0 normal value </param>
    public void ChangeValueMusic(string key, float value)
    {
        _audioMixerGroup.audioMixer.SetFloat(key, Mathf.Lerp(-80, 0, value));
    }

    public void OpenCard(float delay=0)
    {
        audioOpen.PlayDelayed(delay);
    }

}
