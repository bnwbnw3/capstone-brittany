using UnityEngine;
using System.Collections;

public class SetIntroCineAudio : MonoBehaviour {
    public float waitTillPlayAudio = 2.0f;
    public float multiplier = 0.5f;
    private bool hasStarted;
	// Use this for initialization
	void Start () {
        audio.volume = GameControl.control.BackgroundMusicVolume * multiplier;
        hasStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!hasStarted)
        {
            StartCoroutine(WaitToPlay());
        }
	}

    IEnumerator WaitToPlay()
    {
        hasStarted = true;
        yield return new WaitForSeconds(waitTillPlayAudio);
        audio.volume = GameControl.control.BackgroundMusicVolume * multiplier;
        audio.Play();
    }
}
