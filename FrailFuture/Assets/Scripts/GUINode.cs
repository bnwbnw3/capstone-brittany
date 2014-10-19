using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GUINode : MonoBehaviour {

    //public List<GUINode> Edges_Connections;
    //public bool isEndNode = false;
    public NeutralityTypes endNodeType = NeutralityTypes.None;
    public float waitTimeTillCloseNode = 0.25f;
    public Material openDoorMaterial;
    public Material closedDoorMaterial;
   // private int index;
   // private bool indexSet;
    private bool passedThrough;

    void Awake()
    {
        this.renderer.material = openDoorMaterial;
       // index = -1;
       // indexSet = false;
        passedThrough = false;
    }

    public void init()
    {
        /* 
       if (!indexSet)
       {
           index = NodeManager.nodeManager.AllNodes.IndexOf(this);
           indexSet = true;
           Tools.AssertTrue(index != -1, "Make sure this node is in Node Manager.");
       }
     if (Edges_Connections.Count == 0)
       {
           //Tools.AssertTrue(isEndNode, "Node has 0 connections, should be an End Node");
           Tools.AssertFalse(endNodeType == NeutralityTypes.None, "Should have an End Node type");
       }
       else
       {
           //Tools.AssertFalse(isEndNode, "Node has multi-connections, should not be an End Node");
           Tools.AssertTrue(endNodeType == NeutralityTypes.None, "Should have no End Node type");
       }
       */
    }

    // Update is called once per frame
    void Update() 
    {
        if (!passedThrough)
        {
            if (collider.isTrigger)
            {
                if (SoundManager.soundManager.getIsAiTalking())
                {
                    closingNode();
                }
            }
            else
            {
                if (!SoundManager.soundManager.getIsAiTalking())
                {
                    openingNode();
                }
            }
        }
	}

    public void OnTriggerExit(Collider c)
    {
        if (collider.isTrigger)
        {
           StartCoroutine(closeNode(waitTimeTillCloseNode));
           passedThrough = true;
        }
    }
    /*public int getIndexFromNodeManager()
    {
        return index;
    }*/
    public void resetGUINode()
    {
        openingNode();
        passedThrough = false;
    }

    public IEnumerator closeNode(float secondsToWait)
    {
        //play next audio for path to choose
        if (endNodeType == NeutralityTypes.None)
        {
            int pickDoor = AILearningSim.AIsim.getNextDirection();
            SoundManager.soundManager.playDirection(pickDoor);
        }
        else
        {
            SoundManager.soundManager.playEndMaze(endNodeType);
        }
        yield return new WaitForSeconds(secondsToWait);
        closingNode();
    }

    void openingNode()
    {
        renderer.material = openDoorMaterial;
        collider.isTrigger = true;
    }

    void closingNode()
    {
        renderer.material = closedDoorMaterial;
        collider.isTrigger = false;
    }
}

