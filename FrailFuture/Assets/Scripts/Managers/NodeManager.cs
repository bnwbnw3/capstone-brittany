using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NodeManager : MonoBehaviour 
{
    public List<GameObject> AllNodes;
    public List<GameObject> Hallways;
    public List<GameObject> objectSpawner;

    public static NodeManager nodeManager;
    private HashSet<int> hallwaysUsed;
    private bool conditionToLoadEndScene;

    private int CountSinceLastVo;
    private int buffer;
    private int TotalCountToSeeVo;


    System.Random rand;
    void Awake()
    {
      nodeManager = this;
      showNextRoom();
      setupAllHallways();
      rand = new System.Random(System.DateTime.Now.GetHashCode());
    }

    private void SetUpHallways()
    {
        if (GameControl.control.NumberOfHallways == 1)
        {
            hallwaysUsed = new HashSet<int>() { 1, 2, 3, 4 };
        }
    }

    public void showNextRoom()
    {
        conditionToLoadEndScene = GameControl.control.CurrentPlayThrough >= GameControl.control.MinNumPlayThroughs && GameControl.control.getAi.getNeutralityState() != NeutralityTypes.Neutral;
        resetObjSpawners();
        if (GameControl.control.getAi.getCurrentGraphIndex() != 0)
        {
            GameControl.control.EndNodeButtonPressed = false;
        }

        if (hallwaysUsed == null)
        {
            //first round, start of maze, only needs 1 hallway
            hallwaysUsed = new HashSet<int>() { 1 ,2, 3, 4 };
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
        else if (hallwaysUsed.Count < GameControl.control.NumberOfHallways)
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
        if (GameControl.control.getAi.getCurrentGraphIndex() == 0)
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
        else if (GameControl.control.getAi.getCurrentGraphIndex() == 0 && !GameControl.control.JustReset)
        {
            objectSpawner[0].SetActive(true);
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                Vector3 newPos = GameObject.Find("maze0Spawner").transform.position;
                player.transform.position = new Vector3(newPos.x, 0.5f, newPos.z);
            }
            showNextHall();
        }
    }

    void loadEndScene()
    {
        setupAllHallways();
        ScreenFader.screenFader.makeSolid("ControlRoom_EndScene", 2.0f);
    }

    void showNextNode()
    {
        CountSinceLastVo++;
        SoundManager.soundManager.playDONADialogue();
        int inputsAvalible = (!GameControl.control.JustReset && GameControl.control.getAi.getNextGraphEndNodeType() != NeutralityTypes.None) ? 0 : GameControl.control.getAi.getNextInputsFromGraph().Length;
        GameControl.control.JustReset = (inputsAvalible == 0);
        string name = "" + inputsAvalible + "DoorGUINode";
        onlyShowNode(name);
    }

    void showNextHall()
    {
        int indexToUse = -1;
        if (CountSinceLastVo >= TotalCountToSeeVo)
        {
            indexToUse = 0;
            CountSinceLastVo = 0;
        }
        else
        {
            var range = Enumerable.Range(0, Hallways.Count).Where(i => !hallwaysUsed.Contains(i));
            int index = rand.Next(0, Hallways.Count - hallwaysUsed.Count);
            indexToUse = range.ElementAt(index);
        }

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
                node.EndNodeType = !GameControl.control.JustReset ? NeutralityTypes.None : GameControl.control.getAi.getNextGraphEndNodeType();
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

    public void setupAllHallways()
    {
        GameControl.control.JustReset = false;
        if (hallwaysUsed != null)
        {
            hallwaysUsed.Clear();
        }
        conditionToLoadEndScene = GameControl.control.CurrentPlayThrough >= GameControl.control.MinNumPlayThroughs && GameControl.control.getAi.getNeutralityState() != NeutralityTypes.Neutral;
        CountSinceLastVo = 0;
        SetUpHallwayToSeeVo();
    }

    private void SetUpHallwayToSeeVo()
    {
        int HallwaysPerPlayThrough = GameControl.control.MaxNumberOfRows + 1;
        buffer = HallwaysPerPlayThrough;
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