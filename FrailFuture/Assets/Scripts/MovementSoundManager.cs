﻿using UnityEngine;
using System.Collections;

public class MovementSoundManager : MonoBehaviour {
    private bool wasAirbourne;
    private bool wasPressingJump;
	// Use this for initialization
    void Awake()
    {
        if (GameControl.control.canSetPlayerStartingTransform)
        {
            GameControl.control.setPlayerStartingTransform(transform.position, transform.eulerAngles, transform.localScale);
        }
        wasAirbourne = false;
        wasPressingJump = false;
    }
    void Start() { }
	// Update is called once per frame
	void Update () 
    {
        bool spaceDown = Input.GetKeyDown(KeyCode.Space);
       
        if (!wasPressingJump && spaceDown)
        {
            wasPressingJump = true;
        }

        bool onGround = GetComponent<CharacterMotor>().grounded;

        if (onGround)
        {
            if (wasAirbourne)
            {
                wasAirbourne = false;
                SoundManager.soundManager.playLandSound();
            }
            else if (wasPressingJump)
            {
                wasPressingJump = false;
                SoundManager.soundManager.playJumpSound();
            }
            if ((getIfWASDKeysDown() || getIfWASDKeysHeld() || getIfARROWKeysDown() || getIfARROWKeysHeld()) && (!audio.isPlaying || audio.volume == 0.0f))
            {
                SoundManager.soundManager.playWalkSound();
            }
            else if (!getIfWASDKeysHeld() && !getIfARROWKeysHeld())
            {
                SoundManager.soundManager.stopPlayerSounds();
            }
        }
        else
        {
            wasAirbourne = true; 
        }
	}

    private bool getIfWASDKeysDown()
    {
       bool WDown = Input.GetKeyDown(KeyCode.W);
       bool ADown = Input.GetKeyDown(KeyCode.A);
       bool SDown = Input.GetKeyDown(KeyCode.S);
       bool DDown = Input.GetKeyDown(KeyCode.D);

        return (WDown || ADown || SDown || DDown) ;
    }
    private bool getIfARROWKeysDown()
    {
        bool WDown = Input.GetKeyDown(KeyCode.UpArrow);
        bool ADown = Input.GetKeyDown(KeyCode.LeftArrow);
        bool SDown = Input.GetKeyDown(KeyCode.DownArrow);
        bool DDown = Input.GetKeyDown(KeyCode.RightArrow);

        return (WDown || ADown || SDown || DDown);
    }
    private bool getIfWASDKeysHeld()
    {
       bool WHeld = Input.GetKey(KeyCode.W);
       bool AHeld = Input.GetKey(KeyCode.A);
       bool SHeld = Input.GetKey(KeyCode.S);
       bool DHeld = Input.GetKey(KeyCode.D);

       return (WHeld || AHeld || SHeld || DHeld);
    }
    private bool getIfARROWKeysHeld()
    {
        bool WHeld = Input.GetKey(KeyCode.UpArrow);
        bool AHeld = Input.GetKey(KeyCode.LeftArrow);
        bool SHeld = Input.GetKey(KeyCode.DownArrow);
        bool DHeld = Input.GetKey(KeyCode.RightArrow);

        return (WHeld || AHeld || SHeld || DHeld);
    }
}
