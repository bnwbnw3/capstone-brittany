using UnityEngine;
using System.Collections;

public class AILearningSim : MonoBehaviour {

	// Use this for initialization
    public static AILearningSim AIsim;
	int[] choicesGiven;
	int AiChoice;
    int directionGiven;
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
	}
	
    public void userPicked(int choice)
    {
        GameControl.control.Ai.informOfPick(choice);
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
        directionGiven = GameControl.control.Ai.getDirection();
        //Debug.Log("AI tells you to pick:" + directionGiven + ", but actually wants you to pick: " + AiChoice);
        GameControl.control.GameReady = true;
        return directionGiven;
    }
}
