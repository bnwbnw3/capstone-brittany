using UnityEngine;
using System.Collections;

public class AILearningSim : MonoBehaviour {

	// Use this for initialization
    public static AILearningSim AIsim;
	int[] choicesGiven;
	int AiChoice;
    int directionGiven;
    private string text;
    void Awake()
    {
        if (AIsim == null)
        {
            DontDestroyOnLoad(gameObject);
            AIsim = this;
        }
        else if (AIsim != this)
        {
            Destroy(gameObject);
        }
    }
	void Start () {
        text = "default";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
        //Should now be settup 100% be loading in the NodeManager data
       /* if (!GameControl.control.AiReady)
        {
            GameControl.control.AiReady = true;
        }*/
        //Should now be called when you run into a Node
       /* if (!GameControl.control.GameReady)
        {
            getNextDirection();
        }*/
       /* 
		GUI.Label (new Rect (10, Screen.height - 600, 200, 100), "Please pick a number...\n But you should pick: " + directionGiven);

		int size = 50;
		int index = 0;
		int padding = 10;
		int height = Screen.height - 500;

		int width = 4;
		int spacePerButton = size + padding;

		foreach(int c in choicesGiven)
		{
			int xIndex = index % width;
			int yIndex = index / width;

			int y = height + yIndex * spacePerButton;
			int x = xIndex * spacePerButton;

            string msg = ""+c;
            msg = (c == directionGiven) ? "Pick\nMe!" : "NO!";

            if (GUI.Button(new Rect(x, y, size, size), msg))
			{
                GameControl.control.Ai.informOfPick(c);
				Debug.Log("Player Choice: " + c);
                getNextDirection();
			}
			index++;
		}*/
        text =  GUI.TextField(new Rect(10, Screen.height - 380,100,50), text, 20) ;

        if (GUI.Button(new Rect(10, Screen.height - 280, 100, 100), "Save"))
        {
            GameControl.control.Save(text + ".dat");
        }
        if (GUI.Button(new Rect(10, Screen.height - 180, 100, 100), "Load"))
        {
            GameControl.control.Load(text + ".dat");
            GameControl.control.GameReady = false;
        }
	}
    public void userPicked(int choice)
    {
        GameControl.control.Ai.informOfPick(choice);
        Debug.Log("Player Choice: " + choice);
        //getNextDirection();
    }

	void getRandomChoices()
	{
        int randomL = Random.Range(GameControl.control.minNumChoices, GameControl.control.maxNumChoices + 1);
		choicesGiven = new int[randomL];
		
		for(int i = 0; i< randomL; i++)
		{
			choicesGiven[i] = i+1;
		}

        //decide what AI says to do 
        AiChoice = choicesGiven[Random.Range(0, choicesGiven.Length)];
        directionGiven = AiChoice;
	}

    public int getNextDirection()
    {
        choicesGiven = GameControl.control.Ai.getNextInputsFromGraph();
        AiChoice = GameControl.control.Ai.getAiCurrentDesire();
        directionGiven = GameControl.control.Ai.getDirection();//GameControl.control.Ai.getChoiceToDeliver(choicesGiven, AiCurrentDesire);
        Debug.Log("AI tells you to pick:" + directionGiven + ", but actually wants you to pick: " + AiChoice);
        GameControl.control.GameReady = true;
        return directionGiven;
    }
}
