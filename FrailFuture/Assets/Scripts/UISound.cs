using UnityEngine;
using System.Collections;

public class UISound : MonoBehaviour {

    public AudioClip buttonClick;
    public void playClick()
    {
        audio.PlayOneShot(buttonClick);
    }
}
