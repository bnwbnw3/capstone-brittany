using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NodeManager : MonoBehaviour 
{
    //0 = start node
    //last = very last end node
    //public List<GUINode> AllNodes;
    public List<GameObject> AllNodes;
    public List<GameObject> Hallways;
    HashSet<int> hallwaysUsed;
    public static NodeManager nodeManager;
    private bool justReset;
    private System.Random rand;
	// Use this for initialization
    void Awake()
    {
        if (nodeManager == null)
        {
            DontDestroyOnLoad(gameObject);
            nodeManager = this;
            init();
            justReset = false;
        }
        else if (nodeManager != this)
        {
            Destroy(gameObject);
        }
    }

    void init()
    {
        showNextRoom();
    }

    public void showNextRoom()
    {
        if (hallwaysUsed == null)
        {
            //first round, start of maze, only needs 1 hallway
            hallwaysUsed = new HashSet<int>() { 3, 4 };
        }

        //testing Nodes
        showNextNode();
        /*
        if (hallwaysUsed.Count <= 2)
        {
            showNextHall();
        }
        else
        {
            showNextNode();
        }*/
    }

    void showNextNode()
    {
        int inputsAvalible = (!justReset && GameControl.control.Ai.getNextGraphEndNodeType() != NeutralityTypes.None) ? 0 : GameControl.control.Ai.getNextInputsFromGraph().Length;
        justReset = (inputsAvalible == 0);
        string name = "" + inputsAvalible + "DoorGUINode";
        onlyShowNode(name);
    }

    void showNextHall()
    {
        var range = Enumerable.Range(0, Hallways.Count).Where(i => !hallwaysUsed.Contains(i));
        var rand = new System.Random();
        int index = rand.Next(0, Hallways.Count - hallwaysUsed.Count);
        int indexToUse = range.ElementAt(index);
        hallwaysUsed.Add(indexToUse);
        onlyShowHallway(indexToUse);
    }

    void onlyShowNode(string name)
    {
        hallwaysUsed.Clear();
        for (int i = 0; i < AllNodes.Count; i++)
        {
            if (AllNodes[i].name == name)
            {
                AllNodes[i].SetActive(true);
                //AllNodes[i].GetComponentInChildren<GUINode>().resetGUINode();
                GameObject go = AllNodes[i];
                GUINode node = (GUINode)go.GetComponentInChildren<GUINode>();
                node.endNodeType = !justReset ? NeutralityTypes.None : GameControl.control.Ai.getNextGraphEndNodeType();
                node.resetGUINode();
            }
            else
            {
                AllNodes[i].SetActive(false);
            }
        }
        for (int i = 0; i < Hallways.Count; i++)
        {
            Hallways[i].SetActive(false);
        }
    }

    void onlyShowHallway(int index)
    {
        for (int i = 0; i < Hallways.Count; i++)
        {
            //if active and not the node we want, deActivate it.
            Hallways[i].SetActive((Hallways[i].activeInHierarchy != true && i == index) || i ==index);
        }
        for (int i = 0; i < AllNodes.Count; i++)
        {
            AllNodes[i].SetActive(false);
        }
    }

    void Start() 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void resetAllNodes()
    {
        BroadcastMessage("resetGUINode");
    }
}
