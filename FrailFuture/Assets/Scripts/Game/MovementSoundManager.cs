using UnityEngine;
using System.Collections;

public class MovementSoundManager : MonoBehaviour 
{
    private bool wasAirbourne;
    private bool wasPressingJump;
    public GameObject playerCamera;
    public GameObject cameraThatFollowsPlayer;

    void Awake()
    {
        if (GameControl.control.StartingPlayerVars != null)
        {
            GameControl.control.StartingPlayerVars.pos = transform.position;
            GameControl.control.StartingPlayerVars.eulerAngles = transform.eulerAngles;
            GameControl.control.StartingPlayerVars.scale = transform.localScale;
        }
        wasAirbourne = false;
        wasPressingJump = false;

        if (cameraThatFollowsPlayer != null)
        {
            if (playerCamera.camera != cameraThatFollowsPlayer.GetComponent<FollowCamera>().followThis)
            {
                cameraThatFollowsPlayer.GetComponent<FollowCamera>().followThis = playerCamera.camera;
            }
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (cameraThatFollowsPlayer != null)
        {
            FollowCamera fc = cameraThatFollowsPlayer.GetComponent<FollowCamera>();
            if (fc != null)
            {
                fc.manualUpdateAll();
            }
        }

        bool spaceDown = Input.GetKeyDown(KeyCode.Space);
       
        if (!wasPressingJump && spaceDown)
        {
            wasPressingJump = true;
        }

        bool onGround = GetComponent<CharacterMotor>().grounded;

        if (onGround)
        {
             if (wasPressingJump)
            {
                wasPressingJump = false; 
                 SoundManager.soundManager.playJumpSound();
             }
            else if (wasAirbourne)
            {
                wasAirbourne = false;
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

    public void SetCameraFlags(UnityEngine.CameraClearFlags flag)
    {
        playerCamera.camera.clearFlags = flag;
        if (cameraThatFollowsPlayer != null)
        {
            cameraThatFollowsPlayer.GetComponent<FollowCamera>().manualUpdateAll();
        }
    }
    //Private Get-ers
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