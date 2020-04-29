using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundControler : MonoBehaviour
{
    public float volume;
    private float save_volume;

    private Slider slider;
    private GameObject button;

    public Sprite[] on_off_box = new Sprite[2];
    Sprite button_img; //on/off button

    public void Initialize()
    {
        slider = gameObject.transform.Find("slider").gameObject.GetComponent<Slider>();
        button = gameObject.transform.Find("button").gameObject;
        button_img = button.GetComponent<Image>().sprite;

        slider.value = volume;
        Refresh(on_off_box);
    }

    private void Refresh(Sprite[] on_off_box)
    {
        button_img = (volume == 0) ? on_off_box[1] : on_off_box[0];
        button.GetComponent<Image>().sprite = button_img;
    }

    public void OnSoundSliderBox(SoundControler sound)
    {
        sound.volume = sound.slider.value;
        sound.Refresh(on_off_box);
    }

    public void OnOnOffBox(SoundControler sound)
    {
        if (sound.volume == 0)  //OFF
        {
            sound.volume = sound.save_volume;
        }
        else // if (sound.volume != 0) ON
        {
            sound.save_volume = sound.volume;
            sound.volume = 0;
        }

        sound.slider.value = sound.volume;
        sound.Refresh(on_off_box);
    }
}
