using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider music;
    [SerializeField] private Slider sounds;

    public void StartValueMusic()
    {
        float value = PlayerPrefs.GetFloat("MasterMusicValue", 1);
        float valueFx = PlayerPrefs.GetFloat("ValueFx", 1);
        music.value = value;
        sounds.value = valueFx;
        GameManager.instance.MusicManager.ChangeValueMusic("MasterMusicValue", value);
        GameManager.instance.MusicManager.ChangeValueMusic("ValueFx", valueFx);
    }
    public void ChangeValueMusic(float value)
    {
        GameManager.instance.MusicManager.ChangeValueMusic("MasterMusicValue", value);
        PlayerPrefs.SetFloat("MasterMusicValue", value);
    }
    public void ChangeValueSound(float value)
    {
        GameManager.instance.MusicManager.ChangeValueMusic("ValueFx", value);
        PlayerPrefs.SetFloat("ValueFx", value);
    }
    
}
