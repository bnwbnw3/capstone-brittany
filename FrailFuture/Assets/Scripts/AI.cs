using System;
using System.Collections.Generic;
using System.Linq;

public class AI
{
       private Neutrality _neutrality;
       private Graph _maze;
       private Brain _brain;
       private Dictionary<NeutralityTypes, int> _mazeEndIndexs;
       private PathWayFinder pf;

      private int AiCurrentDesire;
      private int AiDesiredEndIndex;
      private int directionGiven;
      private int[] inputsAvalible;
      private int graphIndex;
      private const float neutralityAdder = 0.1f;
        public AI(Graph maze, Neutrality starting, Brain brain, Dictionary<NeutralityTypes, int> mazeEndIndexs)
        {
            _maze = maze;
            _neutrality = starting;
            _brain = brain;
            _mazeEndIndexs = mazeEndIndexs;
            int lowestEndIndex = mazeEndIndexs.OrderBy(k => k.Value).FirstOrDefault().Value;
            pf = new PathWayFinder(_maze,lowestEndIndex);
            AiCurrentDesire = 0;
            directionGiven = 0;
            AiDesiredEndIndex = _mazeEndIndexs[_neutrality.getState()];
            int numInputs = pf.getNumPossibleInputs(0);
            int[] inputs = new int[numInputs];
            for (int i = 0; i < numInputs; i++)
            {
                inputs[i] = i + 1;
            }
            informOfPick(0);
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
                return _brain.getPlayerActions().Get(_brain.getPlayerActions().Count-1);
            }
            else
            {
                return null;
            }
        }

        public NeutralityTypes getNextGraphEndNodeType()
    {
        NeutralityTypes type = NeutralityTypes.None;
        for (int i = (int)NeutralityTypes.Heavenly; i < (int)NeutralityTypes.COUNT; i++)
        {
            if (_mazeEndIndexs.ContainsKey((NeutralityTypes)i) && _mazeEndIndexs[(NeutralityTypes)i] == graphIndex)
            {
                type = (NeutralityTypes)i;
            }
        }

        return type;
    }

       public void informOfPick(int userChoice)
       {
           if (userChoice != 0)
           {
               _brain.checkUserChoice(userChoice);
               PlayerData pd = getLastPickedInfo();
               if (pd.delivered == pd.picked)
               {
                   _neutrality.Add(neutralityAdder);
               }
               else
               {
                   _neutrality.Add(-neutralityAdder);
               }
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
               //UPDATE NEUTRALITY
               int endIndexWas = pf.endIndex;
               if (endIndexWas == AiDesiredEndIndex)
               {
                   int winner = 3;
               }
               if (endIndexWas == pf.getAvalibleDesiredIndex())
               {
                   int halfWinner = 4;
               }

               //set new neutrality after effects from last game
               AiDesiredEndIndex = _mazeEndIndexs[_neutrality.getState()];
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

       public float getNeutrality()
       {
           return _neutrality.getNeutrality();
       }
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
public class AIData
{
   public BrainData brain;
   public float neutrality;
}
