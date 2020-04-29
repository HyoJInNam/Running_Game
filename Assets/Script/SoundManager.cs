using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PLAYERSOUNDTYPE
{
    MOMSTERMOVE= 0,
    GETSTAR,
    JUMP,
    DEAD,
    WINCE,
    CLEAR
}
public class SoundManager : MonoBehaviour
{
    public GameObject option_obj;
    public GameObject ui_obj;

    public SoundControler music;
    public SoundControler sound;

    public AudioSource bgm;
    public List<AudioSource> sounds;

    public void Setting()
    {
        option_obj = gameObject.transform.Find("option").gameObject;
        ui_obj = option_obj.transform.Find("Audio_settings").gameObject;
        music = ui_obj.transform.Find("music").gameObject.GetComponent<SoundControler>();
        sound = ui_obj.transform.Find("sound").gameObject.GetComponent<SoundControler>();

        GameObject audio_obj = option_obj.transform.Find("Audio").gameObject;
        bgm = audio_obj.transform.Find("bgm").gameObject.GetComponent<AudioSource>();
        for (int i = 0; i < 6; i++)
            sounds.Add(audio_obj.transform.GetChild(i + 1).gameObject.GetComponent<AudioSource>());
    }

    public void Initialize(float musicV, float soundV)
    {
        music.volume = musicV;
        sound.volume = soundV;
        music.Initialize();
        sound.Initialize();
    }

    public void SoundLateUpdate()
    {
        bgm.volume = music.volume;
        foreach (AudioSource s in sounds){
            s.volume = sound.volume;
        }
    }

    public void PlayPlayerSound(PLAYERSOUNDTYPE type)
    {
        switch(type)
        {
            case PLAYERSOUNDTYPE.MOMSTERMOVE:
                sounds[0].Play();
                break;
            case PLAYERSOUNDTYPE.GETSTAR:
                sounds[1].Play();
                break;
            case PLAYERSOUNDTYPE.JUMP:
                sounds[2].Play();
                break;
            case PLAYERSOUNDTYPE.DEAD:
                sounds[3].Play();
                break;
            case PLAYERSOUNDTYPE.WINCE:
                sounds[4].Play();
                break;
            case PLAYERSOUNDTYPE.CLEAR:
                bgm.Stop();
                sounds[5].Play();
                break;
        }
    }

}
