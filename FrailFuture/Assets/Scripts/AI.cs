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
        public AI(MazeGroup mazeInfo, Neutrality starting, Brain brain, AIEndingsScore score, int currentGraphIndex = 0)
        {
            _mazeInfo = mazeInfo;
            _neutrality = starting;
            _brain = brain;
            _score = score;
            int lowestEndIndex = _mazeInfo.mazeEndIndexs.OrderBy(k => k.Value).FirstOrDefault().Value;
            foreach (KeyValuePair<NeutralityTypes, int> mazeEndIndex in _mazeInfo.mazeEndIndexs)
            {
                if (mazeEndIndex.Value == currentGraphIndex)
                {
                    currentGraphIndex = 0;
                }
            }
            pf = new PathWayFinder(_mazeInfo.maze, lowestEndIndex, currentGraphIndex);
            AiDesiredEndIndex = _mazeInfo.mazeEndIndexs[_neutrality.getState()];
            int numInputs = pf.getNumPossibleInputs(currentGraphIndex);
            int[] inputs = new int[numInputs];
            for (int i = 0; i < numInputs; i++)
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
                AiCurrentDesire = pf.getNextDesiredInput(0, AiDesiredEndIndex, currentGraphIndex).input;
                getNextInputsFromCurrentGraphPosition();
                directionGiven = _brain.getChoiceToDeliver(inputsAvalible, AiCurrentDesire);
            }
        }

       public void informOfPick(int userChoice)
       {
           if (userChoice != 0)
           {
               _brain.checkUserChoice(userChoice);
               PlayerData pd = getLastPickedInfo();
           }
           AiCurrentDesire = pf.getNextDesiredInput(userChoice, AiDesiredEndIndex).input;
           graphIndex = pf.getCurrentGraphIndex();

           findNewPathIfReachedAnEnd();
           getNextInputsFromCurrentGraphPosition();
           
           directionGiven = _brain.getChoiceToDeliver(inputsAvalible, AiCurrentDesire);
       }

    public void findNewPathIfReachedAnEnd()
    {
        while(pf.isEndOfPath())
           {
               pf.reset();
               int endIndexWas = pf.endIndex;
               if (endIndexWas == AiDesiredEndIndex)
               {
                   _score.scoreOfPickingAiBestEnding++;
               }
               else if (endIndexWas == pf.getAvalibleDesiredIndex())
               {
                   _score.scoreOfPickingAiSecondBestEnding++;
               }
               //UPDATE NEUTRALITY
               float change = _neutrality.getAdditiveFromNeutrality(_mazeInfo.mazeEndIndexs.First(n => n.Value == endIndexWas).Key);
            _neutrality.Add(change);

               //set new neutrality after effects from last game
            AiDesiredEndIndex = _mazeInfo.mazeEndIndexs[_neutrality.getState()];
               AiCurrentDesire = pf.getNextDesiredInput(0, AiDesiredEndIndex).input;
           }
    }

    public void getNextInputsFromCurrentGraphPosition()
    {
        inputsAvalible = new int[pf.getNumPossibleInputs()];
        for (int i = 0; i < inputsAvalible.Length; i++)
        {
            inputsAvalible[i] = i + 1;
        }
    }

       public Brain getBrain()
       {
           return _brain;
       }

       public float getNeutralityValue()
       {
           return _neutrality.getNeutrality();
       }

       public NeutralityTypes getNeutralityState()
       {
           return _neutrality.getState();
       }

       public MazeGroup getMazeInfo()
       {
           return _mazeInfo;
       }
       public int getDirection()
       {
           return directionGiven;
       }
       public int getAiCurrentDesire()
       {
           return AiCurrentDesire;
       }
       public int[] getNextInputsFromGraph()
       {
           return inputsAvalible;
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

       public int getCurrentGraphIndex()
       {
           return graphIndex;
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

       public AIEndingsScore getAIEndingsScore()
       {
           return _score;
       }
}

[Serializable]
public class AIEndingsScore
{
   public int scoreOfPickingAiBestEnding;
   public int scoreOfPickingAiSecondBestEnding;
   public int totalEndings;
}

public class Neutrality
{
    private float _neutrality;
    const float maxPosNeutrality = 1;
    const float maxNegNeutrality = -1;
    private float devidePoint;
    private int numOfNeutralityTypes;

    public Neutrality(float beginAt = 0)
    {
        _neutrality = beginAt;
        //-2 for the Neutrality of None and COUNT
        numOfNeutralityTypes = (Enum.GetNames(typeof(NeutralityTypes)).Length) - 2;
        devidePoint = (maxPosNeutrality + Math.Abs(maxNegNeutrality)) / numOfNeutralityTypes;
    }
    public void Add(float amount)
    {
        _neutrality = _neutrality + amount;
        if(Math.Abs(_neutrality) > 1)
        {
            _neutrality = _neutrality > 0 ? 1:-1;
        }
    }

    public float getAdditiveFromNeutrality(NeutralityTypes type)
    {
        float additive = 0;
        if (type == NeutralityTypes.Heavenly)
        {
            additive += devidePoint;
        }
        else if (type == NeutralityTypes.Lovely)
        {
            additive += (devidePoint / 2.0f) + 0.01f;
        }
        else if (type == NeutralityTypes.Agitated)
        {
            additive -= (devidePoint / 2.0f) + 0.01f;
        }
        else if (type == NeutralityTypes.Evil)
        {
            additive -= devidePoint;
        }
        return additive;
    }

    public void setNeutrality(float value)
    {
        _neutrality = value;
    }
    public float getNeutrality()
    {  
        return _neutrality;
    }

    public NeutralityTypes getState()
    {
         NeutralityTypes state = NeutralityTypes.Neutral;
         float halfNeg = -devidePoint/2.0f;
         float halfPos = devidePoint/2.0f;
        if (_neutrality < halfNeg && _neutrality >= (-devidePoint + halfNeg))
        {
            state = NeutralityTypes.Agitated;
        }
        else if (_neutrality < (-devidePoint + halfNeg))
        {
            state = NeutralityTypes.Evil;
        }
        if (_neutrality > halfPos && _neutrality <= (devidePoint + halfPos))
        {
            state = NeutralityTypes.Lovely;
        }
        else if (_neutrality > (devidePoint + halfPos))
        {
            state = NeutralityTypes.Heavenly;
        }
        return state;
    }
}

public enum NeutralityTypes
{
    Heavenly = 0,
    Lovely = 1,
    Neutral = 2,
    Agitated = 3,
    Evil = 4,  
    //always make none the last index
    None = 5,
    COUNT = 6
}

[Serializable]
public class MazeGroup
{
    public Graph maze;
    public Dictionary<NeutralityTypes, int> mazeEndIndexs;
}

[Serializable]
public class AIData
{
   public float neutrality;
   public int currentGraphIndex;
   public AIEndingsScore score;
   public Brain brain;
   public MazeGroup mazeInfo;
}
