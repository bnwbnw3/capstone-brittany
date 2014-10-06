using System;
using System.Collections.Generic;

public class AI
{
       private Neutrality _neutrality;
       private Graph _maze;
       private Brain _brain;

       int AiChoice;
       int directionGiven;
       int PlayerChoice;

        public AI(Graph maze, Neutrality starting, Brain brain)
        {
            _maze = maze;
            _neutrality = starting;
            _brain = brain;
        }

        public int DeliverDirection()
        {
            //get desired node from maze/nodes from game
            //check players position in maze and use info to deliever new directions
            return -1;
        }

       public void informOfPick(int userChoice)
       {
           _brain.checkUserChoice(userChoice);
           //update Ai's knowledge of where the player is
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
