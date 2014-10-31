using UnityEngine;
using System.Collections;

public class updateBackGroundSoundWithGC : MonoBehaviour {

    public float multiplier = 0.25f;

    void Start()
    {
        audio.volume = GameControl.control.backgroundMusicVolume * multiplier;
    }
	void Update () 
    {
        audio.volume = GameControl.control.backgroundMusicVolume * multiplier;
	}
}
