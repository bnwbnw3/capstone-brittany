﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NodeManager : MonoBehaviour 
{
    public List<GameObject> AllNodes;
    public List<GameObject> Hallways;
    public List<GameObject> objectSpawner;

    public static NodeManager nodeManager;
    private HashSet<int> hallwaysUsed;
    public bool JustReset {get;set;}

    void Awake()
    {
      nodeManager = this;
      showNextRoom();
      JustReset = false;
    }

    public void showNextRoom()
    {
        resetObjSpawners();
        if (hallwaysUsed == null)
        {
            //first round, start of maze, only needs 1 hallway
            hallwaysUsed = new HashSet<int>() { 1 ,2, 3, 4 };
            //If initial index is at 0 we are at the begining of the game. Otherwise we 
            //are loading in an old game and we should start the player at the current index

            if (GameControl.control.WasLoaded)
            {
                if (GameControl.control.getAi.getCurrentGraphIndex() == 0)
                {
                    if (GameControl.control.currentPlayThrough > GameControl.control.MinNumPlayThroughs && GameControl.control.getAi.getNeutralityState() != NeutralityTypes.Neutral)
                    {
                        //show "game over/done" screen
                    }
                    else
                    {
                        showNextHall();
                    }
                    
                }
                else
                {
                    showNextNode();
                }
            }
            else
            {
                if (GameControl.control.currentPlayThrough > GameControl.control.MinNumPlayThroughs && GameControl.control.getAi.getNeutralityState() != NeutralityTypes.Neutral)
                {
                    //show "game over/done" screen
                }
                else if (GameControl.control.getAi.getCurrentGraphIndex() == 0 && !JustReset)
                {
                    objectSpawner[0].SetActive(true);
                    GameObject player = GameObject.Find("Player");
                    if (player != null)
                    {
                        Vector3 newPos = GameObject.Find("maze0Spawner").transform.position;
                        player.transform.position = new Vector3(newPos.x, 0.5f,newPos.z);
                    }
                    showNextHall();
                }
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
        SoundManager.soundManager.playDONADialogue();
        int inputsAvalible = (!JustReset && GameControl.control.getAi.getNextGraphEndNodeType() != NeutralityTypes.None) ? 0 : GameControl.control.getAi.getNextInputsFromGraph().Length;
        JustReset = (inputsAvalible == 0);
        if (JustReset)
        {
            GameControl.control.currentPlayThrough++;
        }
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
                node.EndNodeType = !JustReset ? NeutralityTypes.None : GameControl.control.getAi.getNextGraphEndNodeType();
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

    public void resetObjSpawners()
    {
        for (int i = 0; i < objectSpawner.Count; i++)
        {
            objectSpawner[i].SetActive(false);
        }
    }
}