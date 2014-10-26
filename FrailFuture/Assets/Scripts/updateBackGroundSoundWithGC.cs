using UnityEngine;
using System.Collections;

public class updateBackGroundSoundWithGC : MonoBehaviour {

	void Update () 
    {
        audio.volume = GameControl.control.backgroundMusicVolume;
	}
}
