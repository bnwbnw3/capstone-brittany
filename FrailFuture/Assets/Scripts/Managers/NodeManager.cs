using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ProBuilder2.Common;
public class NodeManager : MonoBehaviour 
{
    public List<GameObject> AllNodes;
    public List<GameObject> Hallways;
    public List<GameObject> objectSpawner;
    public GameObject closedDoor;

    public static NodeManager nodeManager;
    private HashSet<int> hallwaysUsed;
    private int lastHallwayAdded;
    private bool conditionToLoadEndScene;

    private int CountSinceLastVo;
    private int buffer;
    private int TotalCountToSeeVo;

   // private bool mirrorNextObj;
    private bool beginingOfGame;
    private GameObject dupplicateObj;
    System.Random rand;
    void Awake()
    {
        nodeManager = this;
        setupAllHallways();
        SetUpHallwayToSeeVo();
        showNextRoom();
    }

    private void SetUpHallways()
    {
        if (GameControl.control.NumberOfHallwaySections == 1)
        {
            hallwaysUsed = new HashSet<int>() { 1, 2, 3, 4 };
        }
    }

    public void showNextRoom()
    {
        if (dupplicateObj != null)
        {
            Destroy(dupplicateObj);
        }
        conditionToLoadEndScene = GameControl.control.CurrentPlayThrough >= GameControl.control.MinNumPlayThroughs && GameControl.control.Ai.getNeutralityState() != NeutralityTypes.Neutral;
        beginingOfGame = false;
        resetObjSpawners();
        if (GameControl.control.Ai.getCurrentGraphIndex() != 0)
        {
            GameControl.control.EndNodeButtonPressed = false;
        }



        if (hallwaysUsed == null)
        {
            //first round, start of maze, only needs 1 hallway
            hallwaysUsed = new HashSet<int>() { 1, 2, 3, 4 };
            SetUpHallwayToSeeVo();
            //If initial index is at 0 we are at the begining of the game. Otherwise we 
            //are loading in an old game and we should start the player at the current index

            if (GameControl.control.WasLoaded)
            {
                gameWasLoadedAtBegining();
            }
            else
            {
                gameWasNotLoadedAtBegining();
            }
        }
        else if (conditionToLoadEndScene)
        {
            loadEndScene();
        }
        else if ((hallwaysUsed.Count < GameControl.control.NumberOfHallwaySections) && (hallwaysUsed.Count < Hallways.Count))
        {
            SetUpHallways();
            showNextHall();
        }
        else
        {
            showNextNode();
        }
    }

    void gameWasLoadedAtBegining()
    {
        if (GameControl.control.Ai.getCurrentGraphIndex() == 0)
        {
            //load end scene or hallway
            if (conditionToLoadEndScene)
            {
                loadEndScene();
            }
            else
            {
                //showNextHall();
                showNextNode();
            }

        }
            //load node
        else
        {
            showNextNode();
        }
    }

    void gameWasNotLoadedAtBegining()
    {
        //load end scene?
        if (conditionToLoadEndScene)
        {
            loadEndScene();
        }
            //load beginning room with hospital stuff?
        else if (GameControl.control.Ai.getCurrentGraphIndex() == 0 && !GameControl.control.JustReset)
        {
            beginingOfGame = true;
            objectSpawner[0].SetActive(true);
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                Vector3 newPos = GameObject.Find("maze0Spawner").transform.position;
                player.transform.position = new Vector3(newPos.x, 0.5f, newPos.z);
            }
            //mirrorNextObj = true;
            Hallways[0].GetComponentInChildren<HallwayDialogueVOTrigger>().playVODialogue();
            showNextHall(true);
        }
    }

    void loadEndScene()
    {
        setupAllHallways();
        ScreenFader.screenFader.makeSolid("ControlRoom_EndScene", 2.0f);
    }

    void showNextNode()
    {
        //closedDoor.SetActive(false);
       // mirrorNextObj = true;
        hallwaysUsed.Clear();
        SoundManager.soundManager.playDONADialogue();
        int inputsAvalible = (!GameControl.control.JustReset && GameControl.control.Ai.getNextGraphEndNodeType() != NeutralityTypes.None) ? 0 : GameControl.control.Ai.getNextInputsFromGraph().Length;
        GameControl.control.JustReset = (inputsAvalible == 0);
        string name = "" + inputsAvalible + "DoorGUINode";
        onlyShowNode(name);
    }

    void showNextHall(bool forceShow0Hallway = false)
    {
        //closedDoor.SetActive(!mirrorNextObj);
        int indexToUse = 0;
        if (forceShow0Hallway || (CountSinceLastVo >= TotalCountToSeeVo))
        {
            CountSinceLastVo = 0;
        }
        else
        {
            CountSinceLastVo++;
            //-1 are there because we are not allowing the index 0 hallway to be used.
           List<int> range = Enumerable.Range(1, Hallways.Count - 1).Where(i => i != 0 && !hallwaysUsed.Contains(i)).ToList();
           int index = rand.Next(0, range.Count);
           indexToUse = range[index];
        }

        hallwaysUsed.Add(indexToUse);
        lastHallwayAdded = indexToUse;
        onlyShowHallway(indexToUse);
       // mirrorNextObj = false;
    }

    void onlyShowNode(string name)
    {
        for (int i = 0; i < AllNodes.Count; i++)
        {
            if (AllNodes[i].name == name)
            {
                AllNodes[i].SetActive(true);
                GameObject go = AllNodes[i];
                SetUpNode(go);
                //Mirror Node
                dupplicateObj = (GameObject)ProBuilder.Instantiate(AllNodes[i], new Vector3(2,0,0), new Quaternion(0, 180, 0, 0));
                SetUpNode(dupplicateObj);
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

    void SetUpNode(GameObject node)
    {
        GUINode tempGUINode = (GUINode)node.GetComponentInChildren<GUINode>();
        tempGUINode.EndNodeType = !GameControl.control.JustReset ? NeutralityTypes.None : GameControl.control.Ai.getNextGraphEndNodeType();
        tempGUINode.resetGUINode();
    }

    void onlyShowHallway(int index)
    {
        for (int i = 0; i < Hallways.Count; i++)
        {
            //if active and not the node we want, deActivate it.
            bool setMeActive = (Hallways[i].activeInHierarchy != true && i == index) || i == index;
            Hallways[i].SetActive(setMeActive);

            if (setMeActive)
            {
                int hallIndex = i;

                //if normal hallway or is intro hallway
                if (i != 0 || beginingOfGame)
                {
                    hallIndex = rand.Next(1, Hallways.Count);
                }
                
                Hallways[hallIndex].SetActive(true);
                dupplicateObj = (GameObject)ProBuilder.Instantiate(Hallways[hallIndex], new Vector3(2,0,0), new Quaternion(0, 180, 0, 0));
                if (hallIndex != i)
                {
                    Hallways[hallIndex].SetActive(false);
                }
            }
        }
        for (int i = 0; i < AllNodes.Count; i++)
        {
            AllNodes[i].SetActive(false);
        }
        Hallways[index].BroadcastMessage("resetDummyNode"); 
    }

    public void resetAllNodes()
    {
        BroadcastMessage("resetGUINode");
    }

    public void setupAllHallways()
    {
        rand = new System.Random(System.DateTime.Now.GetHashCode());
        GameControl.control.JustReset = false;
        hallwaysUsed = null;
        conditionToLoadEndScene = GameControl.control.CurrentPlayThrough >= GameControl.control.MinNumPlayThroughs && GameControl.control.Ai.getNeutralityState() != NeutralityTypes.Neutral;
        //mirrorNextObj = false;
        beginingOfGame = false;
        dupplicateObj = null;
    }

    private void SetUpHallwayToSeeVo()
    {
        CountSinceLastVo = 0;
        int HallwaysPerPlayThrough = (GameControl.control.MaxNumberOfRows + 1) * GameControl.control.NumberOfHallwaySections;
        buffer = 0;// HallwaysPerPlayThrough;
        int divisional = (HallwaysPerPlayThrough * GameControl.control.MinNumPlayThroughs) + buffer;
        TotalCountToSeeVo = Mathf.CeilToInt(divisional / SoundManager.soundManager.NumberOfVoDialogue);
    }

    public void resetObjSpawners()
    {
        for (int i = 0; i < objectSpawner.Count; i++)
        {
            objectSpawner[i].SetActive(false);
        }
    }
}