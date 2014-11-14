using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GUINode : Door
{
    public NeutralityTypes EndNodeType { get; set; }
    public float waitTimeTillCloseNode = 0.25f;

    protected override void OnAwake()
    {
 	    base.OnAwake();
        EndNodeType = NeutralityTypes.None;
    }

    public void OnTriggerExit(Collider c)
    {
        if (collider.isTrigger && c.tag == "Player")
        {
           StartCoroutine(closeNode(waitTimeTillCloseNode));
           passedThrough = true;
        }
    }

    public void resetGUINode()
    {
        openingNode();
        passedThrough = false;
    }

    public IEnumerator closeNode(float secondsToWait)
    {
        //play next audio for path to choose
        if (EndNodeType == NeutralityTypes.None)
        {
            int pickDoor = GameControl.control.getAi.getDirection();
            SoundManager.soundManager.playDirection(pickDoor);
        }
        yield return new WaitForSeconds(secondsToWait);
        closingNode();
    }
}