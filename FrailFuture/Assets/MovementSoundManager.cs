using UnityEngine;
using System.Collections;

public class MovementSoundManager : MonoBehaviour {
    private bool walking = false;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        bool WDown = Input.GetKeyDown(KeyCode.W);
        bool ADown = Input.GetKeyDown(KeyCode.A);
        bool SDown = Input.GetKeyDown(KeyCode.S);
        bool DDown = Input.GetKeyDown(KeyCode.D);
        bool WHeld = Input.GetKey(KeyCode.W);
        bool AHeld = Input.GetKey(KeyCode.A);
        bool SHeld = Input.GetKey(KeyCode.S);
        bool DHeld = Input.GetKey(KeyCode.D);

        if ((WDown || ADown || SDown || DDown) && !audio.isPlaying)
        {
            SoundManager.soundManager.playWalkSound(audio);
            walking = true;
        }
        else if (!WHeld && !AHeld && !SHeld && !DHeld)
        {
            SoundManager.soundManager.stopWalkSound(audio);
            walking = false;
        }
	}
}
