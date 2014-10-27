using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NodeManager : MonoBehaviour 
{
    public List<GameObject> AllNodes;
    public List<GameObject> Hallways;
    HashSet<int> hallwaysUsed;
    public static NodeManager nodeManager;
    private bool justReset;
	// Use this for initialization
    void Awake()
    {
            nodeManager = this;
            init();
            justReset = false;
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
            //If initial index is at 0 we are at the begining of the game. Otherwise we 
            //are loading in an old game and we should start the player at the current index
            if (GameControl.control.Ai.getCurrentGraphIndex() == 0)
            {
                showNextHall();
            }
            else
            {
                showNextNode();
            }
        }
        else if (hallwaysUsed.Count <= 2)
        {
            showNextHall();
        }
        else
        {
            showNextNode();
        }
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

    public void resetAllNodes()
    {
        BroadcastMessage("resetGUINode");
    }
}
