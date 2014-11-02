using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Brain
{
   BrainData bd;
   private int[] inputs;
   private int lastDesiredChoice;
   private int lastChoiceToDeliver;

    public Brain(BrainData pastHistory)
    {
        bd = pastHistory;
        checkForKeys();
    }

    public void checkUserChoice(int userChoice)
    {
        bd.totalPossible++;
        picksGivenNumCheck(userChoice);
        picksSpecificNumCheck(userChoice);
        picksDoubleBackNumCheck(userChoice);
        picksFarthestNumCheck(userChoice);
        picksReccesionWrapNumCheck(userChoice);
        picksSuccesionWrapNumCheck(userChoice);

        if (userChoice == lastDesiredChoice)
        {
            bd.scoreOfPickingDesiredInput += 1;
        }
        bd.pastActions.Add(new PlayerData { numOfInputs = inputs.Length, desired = lastDesiredChoice, delivered = lastChoiceToDeliver, picked = userChoice });
    }

    public int getChoiceToDeliver(int[] inputsGiven, int desiredChoice)
    {
        inputs = new int[inputsGiven.Length];
        for (int i = 0; i < inputsGiven.Length; i++)
        {
            inputs[i] = inputsGiven[i];
        }

        lastDesiredChoice = desiredChoice;
        lastChoiceToDeliver = checkForPatterns();
        return lastChoiceToDeliver;
    }

    private int checkForPatterns()
    {
        int final =  picksGivenNumPattern();
        int picksSpecificNumR = picksSpecificNumPattern();
        int picksDoubleBackNumR = picksDoubleBackNumPattern();
        int picksFarthestNumR = picksFarthestNumPattern();
        int picksSuccesionWrapNumR = picksSuccesionWrapNumPattern();
        int picksReccesionWrapNumR = picksReccesionWrapNumPattern();

        if (picksSuccesionWrapNumR != -1)
        {
            final = picksSuccesionWrapNumR;
        }
        else if (picksReccesionWrapNumR != -1)
        {
            final = picksReccesionWrapNumR;
        }
        else if (picksFarthestNumR != -1)
        {
            final = picksFarthestNumR;
        }
        else if (picksDoubleBackNumR != -1)
        {
            final = picksDoubleBackNumR;
        }
        else if (picksSpecificNumR != -1)
        {
            final = picksSpecificNumR;
        }
        if (final == -1)
        {
            Debug.Log("Brain failed to find a direction to give the player");
        }
        return final;
    }

    private int picksGivenNumPattern()
    {
        int toReturn = -1;
        resetPatternIfOver(5, "PicksGivenNum");

        toReturn = lastDesiredChoice;
        if (bd.pastPatterns["PicksGivenNum"] < 0)
        {
            toReturn = grabNextBestNumberMostPicked();
        }

        return toReturn;
    }
    private void picksGivenNumCheck(int userChoice)
    {
        if (userChoice == lastChoiceToDeliver)
        {
            bd.pastPatterns["PicksGivenNum"] += 1;
            bd.pastPatterns["TotalPickedAI"] += 1;
        }
        else
        {
            bd.pastPatterns["PicksGivenNum"] += -1;
            bd.pastPatterns["TotalNotPickedAI"] += 1;
        }
    }

    private int picksSpecificNumPattern()
    {
        int toReturn = -1;
        if (bd.pastPatterns["PicksSpecificNum"] > 2)
        {
            toReturn = bd.pastActions.Get(bd.pastActions.Count - 1).picked;
            bool nextPatternNumIsDesired = toReturn == lastDesiredChoice;

            if (bd.pastPatterns["PicksSpecificNum"] <= 4)
            {
                if (!nextPatternNumIsDesired && bd.pastPatterns["PicksGivenNum"] > 0) 
                {
                    toReturn = -1;
                }
                else if (nextPatternNumIsDesired && bd.pastPatterns["PicksGivenNum"] < 0)
                {
                    toReturn = grabNextBestNumberMostPicked();
                }
            }
        }
        resetPatternIfOver(6, "PicksSpecificNum", 3);
        return toReturn;
    }
    private void picksSpecificNumCheck(int userChoice)
    {
        if(bd.pastActions.Count >= 1)
        {
            checkPatternHelper((userChoice == bd.pastActions.Get(bd.pastActions.Count - 1).picked), "PicksSpecificNum");
        }
        bd.pastPatterns["Picks" + userChoice] += 1;
    }

    private int picksDoubleBackNumPattern()
    {
        int toReturn = -1;
        if (bd.pastPatterns["PicksDoubleBackNum"] > 0)
        {
            int nextInPattern = bd.pastActions.Get(bd.pastActions.Count - 2).picked;
             bool nextPatternNumIsDesired = (nextInPattern == lastDesiredChoice);

             if (!nextPatternNumIsDesired)
             {
                 if(bd.pastPatterns["PicksGivenNum"] < 0)
                 {
                     toReturn = nextInPattern;
                 }
             }
        }
        resetPatternIfOver(5, "PicksDoubleBackNum");
        return toReturn;
    }
    private void picksDoubleBackNumCheck(int userChoice)
    {
        if (bd.pastActions.Count >= 3)
        {
            int pastPicked1 = bd.pastActions.Get(bd.pastActions.Count - 3).picked;
            int pastPicked2 = bd.pastActions.Get(bd.pastActions.Count - 2).picked;
            int pastPicked3 = bd.pastActions.Get(bd.pastActions.Count - 1).picked;
            checkPatternHelper((pastPicked1 == pastPicked3 && pastPicked2 == userChoice), "PicksDoubleBackNum");
            if (pastPicked1 != pastPicked3 && pastPicked2 != userChoice && bd.pastPatterns["PicksDoubleBackNum"] > 0)
            {
                bd.pastPatterns["PicksDoubleBackNum"] = 0;
            }
        }
    }

    private int picksFarthestNumPattern()
    {
        int toReturn = -1;

        if(bd.pastPatterns["PicksFarthestNum"] > 1 && bd.pastPatterns["PicksGivenNum"] < 0 && inputs.Length >= 3)
        {
            if (lastDesiredChoice == inputs[0])
            {
                toReturn = inputs[inputs.Length - 1];
            } 
            else if (lastDesiredChoice == inputs[inputs.Length - 1])
            {
                toReturn = inputs[0];
            }
            else
            {
                if (inputs.Length % 2 != 0)
                {
                   toReturn = (inputs.Length - 1) + 1;
                }
                else
                {
                    toReturn = (inputs.Length - 1) / 2;
                }
                toReturn = toReturn != lastDesiredChoice ? toReturn : -1;
            }
        }
        resetPatternIfOver(5, "PicksFarthestNum",3);
        return toReturn;
    }
    private void picksFarthestNumCheck(int userChoice)
    {
        if (bd.pastActions.Count > 2)
        {
            if (inputs.Length >= 3)
            {
                if (lastChoiceToDeliver == inputs[0])
                {
                    checkPatternHelper((userChoice == inputs[inputs.Length - 1]), "PicksFarthestNum");
                }
                else if (lastChoiceToDeliver == inputs[inputs.Length - 1])
                {
                    checkPatternHelper((userChoice == inputs[0]), "PicksFarthestNum");
                }
            }
        }
    }

    private int picksSuccesionWrapNumPattern()
    {
       int toReturn = -1;
        if(bd.pastPatterns["PicksSuccesionWrapNum"] > 0 && bd.pastPatterns["PicksGivenNum"] < 0 && inputs.Length >= 3)
        {
            toReturn = (lastDesiredChoice -1);
            toReturn = toReturn >= inputs[0] ? toReturn : inputs[inputs.Length - 1];
        }
        resetPatternIfOver(4, "PicksSuccesionWrapNum");
        return toReturn;
    }
    private void picksSuccesionWrapNumCheck(int userChoice)
    {
        int checkAt = 3;
        if (bd.pastActions.Count >= checkAt && inputs.Length >= 3)
        {
            bool pickedPlusOne = false;
            for (int i = checkAt; i > 0 && pickedPlusOne; i--)
            {
                PlayerData current = bd.pastActions.Get(bd.pastActions.Count - i);
                int shouldBe = (current.delivered + 1);
                shouldBe = shouldBe <= inputs[inputs.Length-1] ? shouldBe : inputs[0];
                pickedPlusOne = current.picked == shouldBe;
            }
            checkPatternHelper(pickedPlusOne, "PicksSuccesionWrapNum");
        }
    }

    private int picksReccesionWrapNumPattern()
    {
        int toReturn = -1;
        if (bd.pastPatterns["PicksReccesionWrapNum"] > 0 && bd.pastPatterns["PicksGivenNum"] < 0 && inputs.Length >= 3)
        {
            toReturn = (lastDesiredChoice + 1);
            toReturn = toReturn <= inputs[inputs.Length-1] ? toReturn : inputs[0] ;
        }
        resetPatternIfOver(4, "PicksReccesionWrapNum");
        return toReturn;
    }
    private void picksReccesionWrapNumCheck(int userChoice)
    {
        int checkAt = 3;
        if (bd.pastActions.Count >= checkAt && inputs.Length >= 3)
        {
            bool pickedPlusOne = false;
            for (int i = checkAt; i > 0 && pickedPlusOne; i--)
            {
                PlayerData current = bd.pastActions.Get(bd.pastActions.Count - i);
                int shouldBe = (current.delivered - 1);
                shouldBe = shouldBe >= inputs[0] ? shouldBe :inputs[inputs.Length-1];
                pickedPlusOne = current.picked == shouldBe;
            }
            checkPatternHelper(pickedPlusOne, "PicksReccesionWrapNum");
        }
    }

    private void checkPatternHelper(bool condition, string key)
    {
        if (condition)
        {
            bd.pastPatterns[key] += 1;
        }
        else
        {
            if (bd.pastPatterns[key] > 0)
            {
                bd.pastPatterns[key] += -1;
            }
        }
    }

    private void resetPatternIfOver(int threshHold, string key, int restartPositive = 1, int restartNegative = -1)
    {
        if (Math.Abs(bd.pastPatterns[key]) >= threshHold)
        {
            bd.pastPatterns[key] = bd.pastPatterns[key] > 0 ? restartPositive : restartNegative;
        }
    }

    private int grabNextBestNumberDumb()
    {
        int toReturn = lastDesiredChoice;
        while (toReturn == lastDesiredChoice)
        {
            if (inputs.Length > 2)
            {
                toReturn = new System.Random().Next(1, inputs.Length + 1);
            }
            else
            {
                toReturn = grabOppositeNumOutOfTwo();
            }
        }
        return toReturn;
    }

    //picks next best number that is not the lastDesiredNumber
    private int grabNextBestNumberMostPicked()
    {
        int toReturn = lastDesiredChoice;
        //use data of which numbers user likes to use to choose next best number to deliver.
        if (inputs.Length <= 2)
        {
            toReturn = grabOppositeNumOutOfTwo();
        }
        else
        {
                int highestCount = 0;
                int highestIndex = -1;
                for(int i = 1; i <= inputs.Length; i++)
                {
                    if(highestCount < bd.pastPatterns["Picks" + i] && i != toReturn)
                    {
                        highestIndex = i;
                        highestCount = bd.pastPatterns["Picks" + i];
                    }
                }
                toReturn = highestIndex;
        }
        return toReturn;
    }

    private int grabOppositeNumOutOfTwo()
    {
        int index = lastDesiredChoice - 1 > 0 ? 0 : 1;
        return inputs[index];
    }

    private void checkForKeys()
    {
        addKeyToPatternCount("PicksGivenNum");
        addKeyToPatternCount("TotalPickedAI");
        addKeyToPatternCount("TotalNotPickedAI");
        addKeyToPatternCount("PicksSpecificNum");

        //Be able to count how many times each input is chosen
        for (int i = 0; i <= GameControl.control.maxNumChoices; i++)
        {
            addKeyToPatternCount("Picks" + (i));
        }

        addKeyToPatternCount("PicksDoubleBackNum");
        addKeyToPatternCount("PicksFarthestNum");
        addKeyToPatternCount("PicksSuccesionWrapNum");
        addKeyToPatternCount("PicksReccesionWrapNum");
    }

    //adds if doesn't exist
    private void addKeyToPatternCount(string key, int initialCount = 0)
    {
         if (!bd.pastPatterns.ContainsKey(key))
        {
            bd.pastPatterns.Add(key, initialCount);
        }
    }

    //Get-ers
    public Dictionary<string, int> getPatternCount()
    {
        return bd.pastPatterns;
    }
    public SizedList<PlayerData> getPlayerActions()
    {
        return bd.pastActions;
    }
    public int getScore()
    {
        return bd.scoreOfPickingDesiredInput;
    }
    public int getTotalPossible()
    {
        return bd.totalPossible;
    }
}

[Serializable]
public class PlayerData
{
    public int numOfInputs;
    public int desired;
    public int delivered;
    public int picked;
}


[Serializable]
public class BrainData
{
    public Dictionary<string, int> pastPatterns;
    public SizedList<PlayerData> pastActions;
    public int scoreOfPickingDesiredInput;
    public int totalPossible;
}