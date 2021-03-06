﻿using System;

public class Neutrality
{
    private const float maxPosNeutrality = 1;
    private const float maxNegNeutrality = -1;
    private int numOfNeutralityTypes;
    public float Value { get; set; }
    public float DevidePoint {private set; get;}

    public Neutrality(float beginAt = 0)
    {
        Value = beginAt;
        //-2 for the Neutrality of None and COUNT
        numOfNeutralityTypes = (Enum.GetNames(typeof(NeutralityTypes)).Length) - 2;
        DevidePoint = (maxPosNeutrality + Math.Abs(maxNegNeutrality)) / numOfNeutralityTypes;
    }
    public Neutrality(NeutralityTypes Type)
    {
        //-2 for the Neutrality of None and COUNT
        numOfNeutralityTypes = (Enum.GetNames(typeof(NeutralityTypes)).Length) - 2;
        DevidePoint = (maxPosNeutrality + Math.Abs(maxNegNeutrality)) / numOfNeutralityTypes;
        setValueFromNeutrality(Type);
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
            additive += DevidePoint;
        }
        else if (type == NeutralityTypes.Lovely)
        {
            additive += (DevidePoint / 2.0f);
        }
        else if (type == NeutralityTypes.Agitated)
        {
            additive -= (DevidePoint / 2.0f);
        }
        else if (type == NeutralityTypes.Evil)
        {
            additive -= DevidePoint;
        }
        return additive;
    }

    //Get-ers
    public NeutralityTypes getState()
    {
        NeutralityTypes state = NeutralityTypes.Neutral;
        float halfNeg = -DevidePoint / 2.0f;
        float halfPos = DevidePoint / 2.0f;
        if (Value <= halfNeg && Value > (-DevidePoint + halfNeg))
        {
            state = NeutralityTypes.Agitated;
        }
        else if (Value <= (-DevidePoint + halfNeg))
        {
            state = NeutralityTypes.Evil;
        }
        if (Value >= halfPos && Value < (DevidePoint + halfPos))
        {
            state = NeutralityTypes.Lovely;
        }
        else if (Value >= (DevidePoint + halfPos))
        {
            state = NeutralityTypes.Heavenly;
        }
        return state;
    }

    private void setValueFromNeutrality(NeutralityTypes neutrality)
    {
        float halfNeg = -DevidePoint / 2.0f;
        float halfPos = DevidePoint / 2.0f;

        if (neutrality == NeutralityTypes.Agitated)
        {
            Value = halfNeg;
        }
        else if (neutrality == NeutralityTypes.Evil)
        {
            Value = -DevidePoint + halfNeg;
        }
        else if (neutrality == NeutralityTypes.Lovely)
        {
            Value = halfPos;
        }
        else if (neutrality == NeutralityTypes.Heavenly)
        {
            Value = DevidePoint + halfPos;
        }
    }
}