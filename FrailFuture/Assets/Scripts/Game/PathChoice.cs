using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PathChoice : Door
{
    public float waitTimeTillClosePath= 0.25f;
    public int pathNum;

    public void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            GameControl.control.Ai.informOfPick(pathNum);
            SoundManager.soundManager.playResponse();
        }
    }
    public void resetPath()
    {
        passedThrough = false;
        openingNode();
    }
}