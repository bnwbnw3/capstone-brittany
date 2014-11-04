using System;

public class Neutrality
{
    private const float maxPosNeutrality = 1;
    private const float maxNegNeutrality = -1;
    private float devidePoint;
    private int numOfNeutralityTypes;
    public float Value { get; set; }

    public Neutrality(float beginAt = 0)
    {
        Value = beginAt;
        //-2 for the Neutrality of None and COUNT
        numOfNeutralityTypes = (Enum.GetNames(typeof(NeutralityTypes)).Length) - 2;
        devidePoint = (maxPosNeutrality + Math.Abs(maxNegNeutrality)) / numOfNeutralityTypes;
    }

    public void Add(float amount)
    {
        Value = Value + amount;
        if (Math.Abs(Value) > 1)
        {
            Value = Value > 0 ? 1 : -1;
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

    //Get-ers
    public NeutralityTypes getState()
    {
        NeutralityTypes state = NeutralityTypes.Neutral;
        float halfNeg = -devidePoint / 2.0f;
        float halfPos = devidePoint / 2.0f;
        if (Value < halfNeg && Value >= (-devidePoint + halfNeg))
        {
            state = NeutralityTypes.Agitated;
        }
        else if (Value < (-devidePoint + halfNeg))
        {
            state = NeutralityTypes.Evil;
        }
        if (Value > halfPos && Value <= (devidePoint + halfPos))
        {
            state = NeutralityTypes.Lovely;
        }
        else if (Value > (devidePoint + halfPos))
        {
            state = NeutralityTypes.Heavenly;
        }
        return state;
    }
}