using System;
using System.Collections.Generic;
using System.Linq;

public class AI
{
    private Neutrality _neutrality;
    private MazeGroup _mazeInfo;
    private Brain _brain;
    private AIEndingsScore _score;
    private PathWayFinder pf;

    private int AiCurrentDesire;
    private int AiDesiredEndIndex;
    private int directionGiven;
    private int[] inputsAvalible;
    private int graphIndex;
    private const float neutralityAdder = 0.1f;

    public AI()
      {
          SizedList<PlayerData> temp = new SizedList<PlayerData>(10);
          BrainData bd = new BrainData() { pastPatterns = new Dictionary<string, int>(), pastActions = temp };
          Brain brain = new Brain(bd);
          Neutrality neutrality = new Neutrality(0.0f);
          AIEndingsScore score = new AIEndingsScore();

          ConstructorHelper(MazeGenerator.getAProcedurallyGenMaze(), neutrality, brain, score);
      }
    public AI(MazeGroup mazeInfo, Neutrality starting, Brain brain, AIEndingsScore score, int currentGraphIndex = 0)
    {
        ConstructorHelper(mazeInfo, starting, brain, score, currentGraphIndex);
    }
    private void ConstructorHelper(MazeGroup mazeInfo, Neutrality starting, Brain brain, AIEndingsScore score, int currentGraphIndex = 0)
    {
        _mazeInfo = mazeInfo;
        _neutrality = starting;
        _brain = brain;
        _score = score;
        int lowestEndIndex = _mazeInfo.mazeEndIndexs.OrderBy(k => k.Value).FirstOrDefault().Value;

        if (GameControl.control.JustReset && GameControl.control.EndNodeButtonPressed)
        {
            foreach (KeyValuePair<NeutralityTypes, int> mazeEndIndex in _mazeInfo.mazeEndIndexs)
            {
                if (mazeEndIndex.Value == currentGraphIndex)
                {
                    currentGraphIndex = 0;
                    GameControl.control.JustReset = false;
                }
            }
        }
        pf = new PathWayFinder(_mazeInfo.maze, lowestEndIndex, currentGraphIndex);
        AiDesiredEndIndex = _mazeInfo.mazeEndIndexs[_neutrality.getState()];
        int[] inputs = new int[pf.findNumPossibleInputs(currentGraphIndex)];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = i + 1;
        }

        if (currentGraphIndex == 0)
        {
            directionGiven = 0;
            AiCurrentDesire = 0;
            informOfPick(0);
        }
        else
        {
            graphIndex = currentGraphIndex;
            AiCurrentDesire = pf.findNextDesiredInput(0, AiDesiredEndIndex, getNeutralityValue(), currentGraphIndex).input;
            setNextInputsFromCurrentGraphPosition();
            directionGiven = _brain.getChoiceToDeliver(inputsAvalible, AiCurrentDesire);
        }
    }

    public void informOfPick(int userChoice)
       {
           if (userChoice != 0)
           {
               _brain.checkUserChoice(userChoice);
           }
           AiCurrentDesire = pf.findNextDesiredInput(userChoice, AiDesiredEndIndex, getNeutralityValue()).input;
           graphIndex = pf.getCurrentGraphIndex();

           if (!pf.getIsEndOfPath())
           {
               setNextInputsFromCurrentGraphPosition();
               directionGiven = _brain.getChoiceToDeliver(inputsAvalible, AiCurrentDesire);
           }
       }
    public void findNewPathIfReachedAnEnd()
    {
        if (pf.getIsEndOfPath())
           {
               pf.reset();
               int endIndexWas = pf.getEndIndex();
               _score.totalEndings++;
               if (endIndexWas == AiDesiredEndIndex)
               {
                   _score.scoreOfPickingAiBestEnding++;
               }
               else if (endIndexWas == pf.getAvalibleDesiredIndex())
               {
                   _score.scoreOfPickingAiSecondBestEnding++;
               }
              
               //set new neutrality after effects from last game
                AiDesiredEndIndex = _mazeInfo.mazeEndIndexs[_neutrality.getState()];
                AiCurrentDesire = pf.findNextDesiredInput(0, AiDesiredEndIndex, getNeutralityValue()).input;
               graphIndex = 0;

               setNextInputsFromCurrentGraphPosition();
               directionGiven = _brain.getChoiceToDeliver(inputsAvalible, AiCurrentDesire);
           }
    }

    public bool editNeutrality(NeutralityTypes neuTypeToGetAdditive)
    {
        bool canUpdate = pf.getIsEndOfPath();
        if (canUpdate)
        {
            //UPDATE NEUTRALITY
            float change = _neutrality.getAdditiveFromNeutrality(neuTypeToGetAdditive);
            _neutrality.Add(change);
        }
        return canUpdate;
    }

    public bool editNeutrality(float additive)
    {
        bool canUpdate = pf.getIsEndOfPath();
        if (canUpdate)
        {
            //UPDATE NEUTRALITY
            _neutrality.Add(additive);
        }
        return canUpdate;
    }

    private void setNextInputsFromCurrentGraphPosition()
    {
        inputsAvalible = new int[pf.findNumPossibleInputs()];
        for (int i = 0; i < inputsAvalible.Length; i++)
        {
            inputsAvalible[i] = i + 1;
        }
    }

    //Get-ers
    public int getDirection()
    {
        return directionGiven;
    }
    public int getAiCurrentDesire()
    {
        return AiCurrentDesire;
    }
    public int getCurrentGraphIndex()
    {
        return graphIndex;
    }
    public float getNeutralityValue()
    {
        return _neutrality.Value;
    }
    public int[] getNextInputsFromGraph()
    {
        return inputsAvalible;
    }
    public NeutralityTypes getNeutralityState()
    {
        return _neutrality.getState();
    }
    public NeutralityTypes getNextGraphEndNodeType()
    {
        NeutralityTypes type = NeutralityTypes.None;
        for (int i = (int)NeutralityTypes.Heavenly; i < (int)NeutralityTypes.COUNT; i++)
        {
            if (_mazeInfo.mazeEndIndexs.ContainsKey((NeutralityTypes)i) && _mazeInfo.mazeEndIndexs[(NeutralityTypes)i] == graphIndex)
            {
                type = (NeutralityTypes)i;
            }
        }

        return type;
    }
    public MazeGroup getMazeInfo()
    {
        return _mazeInfo;
    }
    public Brain getBrain()
    {
        return _brain;
    }
    public PlayerData getLastPickedInfo()
    {
        if (_brain.getPlayerActions().Count > 0)
        {
            return _brain.getPlayerActions().Get(_brain.getPlayerActions().Count - 1);
        }
        else
        {
            return null;
        }
    }
    public AIEndingsScore getAIEndingsScore()
    {
        return _score;
    }
}