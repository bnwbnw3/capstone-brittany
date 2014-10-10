using UnityEngine;
using System.Collections;

public class MovementSoundManager : MonoBehaviour {
	// Use this for initialization
    void Awake()
    {
        if (GameControl.control.canSetPlayerStartingTransform)
        {
            GameControl.control.setPlayerStartingTransform(transform.position, transform.eulerAngles, transform.localScale);
        }
    }
    void Start() { }
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
        }
        else if (!WHeld && !AHeld && !SHeld && !DHeld)
        {
            SoundManager.soundManager.stopWalkSound(audio);
        }
	}
}
