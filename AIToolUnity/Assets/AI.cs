﻿using System;
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

       public void informOfPick(int userChoice)
       {
           if (userChoice != 0)
           {
               _brain.checkUserChoice(userChoice);
           }
           AiCurrentDesire = pf.getNextDesiredInput(userChoice, AiDesiredEndIndex).input;

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
    }
    public void Add(float amount)
    {
        _neutrality = _neutrality + amount;
        if(Math.Abs(_neutrality) > 1)
        {
            _neutrality = _neutrality > 0 ? 1:-1;
            numOfNeutralityTypes = Enum.GetNames(typeof(NeutralityTypes)).Length;
            devidePoint = (maxPosNeutrality + Math.Abs(maxNegNeutrality)) / numOfNeutralityTypes;
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
        if (_neutrality < -devidePoint/2 && _neutrality >= (-devidePoint + -devidePoint/2))
        {
            state = NeutralityTypes.Agitated;
        }
        else if (_neutrality < (-devidePoint + -devidePoint / 2))
        {
            state = NeutralityTypes.Evil;
        }
        if (_neutrality > devidePoint / 2 && _neutrality <= (devidePoint + devidePoint / 2))
        {
            state = NeutralityTypes.Lovely;
        }
        else if (_neutrality > (devidePoint + devidePoint / 2))
        {
            state = NeutralityTypes.Heavenly;
        }
        return state;
    }

}

public enum NeutralityTypes
{
    Heavenly,
    Lovely,
    Neutral,
    Agitated,
    Evil, 
    None
}

[Serializable]
public class AIData
{
   public BrainData brain;
   public float neutrality;
}
