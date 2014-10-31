using UnityEngine;
using System.Collections;

public class UISound : MonoBehaviour {

    public AudioClip buttonClick;
    public AudioClip buttonAcceptedClick;
    public void playClick()
    {
        audio.volume = GameControl.control.soundEffectsVolume;
        audio.PlayOneShot(buttonClick);
    }

    public void playAcceptedClick()
    {
        audio.volume = GameControl.control.soundEffectsVolume;
        audio.PlayOneShot(buttonAcceptedClick);
    }
}
