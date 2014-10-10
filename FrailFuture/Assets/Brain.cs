using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public int score;
        public int totalPossible;
    }

    public class Brain
    {
       private Dictionary<string, int> patternCount;
       private SizedList<PlayerData> playerActions;
       private int[] inputs;
       private int lastDesiredChoice;
       private int lastChoiceToDeliver;
       private int score;
       private int totalPossible;

        public Brain(BrainData pastHistory)
        {
            patternCount = pastHistory.pastPatterns;
            playerActions = pastHistory.pastActions;
            checkForKeys();
            score = pastHistory.score;
            totalPossible = pastHistory.totalPossible;
        }

        public void checkUserChoice(int userChoice)
        {
            totalPossible++;
            picksGivenNumCheck(userChoice);
            picksSpecificNumCheck(userChoice);
            picksDoubleBackNumCheck(userChoice);
            picksFarthestNumCheck(userChoice);
            picksReccesionWrapNumCheck(userChoice);
            picksSuccesionWrapNumCheck(userChoice);

            if (userChoice == lastDesiredChoice)
            {
                score += 1;
            }
            playerActions.Add( new PlayerData {numOfInputs = inputs.Length, desired = lastDesiredChoice, delivered = lastChoiceToDeliver, picked = userChoice });
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
                //something went wrong
                int four = 1;
            }
            return final;
        }

        private int picksGivenNumPattern()
        {
            int toReturn = -1;
            resetPatternIfOver(5, "PicksGivenNum");

            toReturn = lastDesiredChoice;
            if (patternCount["PicksGivenNum"] < 0)
            {
                toReturn = grabNextBestNumberMostPicked();
            }

            return toReturn;
        }
        private void picksGivenNumCheck(int userChoice)
        {
            if (userChoice == lastChoiceToDeliver)
            {
                patternCount["PicksGivenNum"] += 1;
                patternCount["TotalPickedAI"] += 1;
            }
            else
            {
                patternCount["PicksGivenNum"] += -1;
                patternCount["TotalNotPickedAI"] += 1;
            }
        }

        private int picksSpecificNumPattern()
        {
            int toReturn = -1;
            if (patternCount["PicksSpecificNum"] > 2)
            {
                toReturn = playerActions.Get(playerActions.Count - 1).picked;
                bool nextPatternNumIsDesired = toReturn == lastDesiredChoice;

                if (patternCount["PicksSpecificNum"] <= 4)
                {
                    if (!nextPatternNumIsDesired && patternCount["PicksGivenNum"] > 0) 
                    {
                        toReturn = -1;
                    }   
                    else if (nextPatternNumIsDesired && patternCount["PicksGivenNum"] < 0)
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
            if(playerActions.Count >= 1)
            {
                patternCount["PicksSpecificNum"] += (userChoice == playerActions.Get(playerActions.Count - 1).picked) ? 1: -1;
            }
            patternCount["Picks" + userChoice] += 1;
        }

        private int picksDoubleBackNumPattern()
        {
            int toReturn = -1;
            if (patternCount["PicksDoubleBackNum"] > 0)
            {
                int nextInPattern = playerActions.Get(playerActions.Count - 2).picked;
                 bool nextPatternNumIsDesired = (nextInPattern == lastDesiredChoice);

                 if (!nextPatternNumIsDesired)
                 {
                     if(patternCount["PicksGivenNum"] < 0)
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
            if (playerActions.Count >= 3)
            {
                int pastPicked1 = playerActions.Get(playerActions.Count - 3).picked;
                int pastPicked2 = playerActions.Get(playerActions.Count - 2).picked;
                int pastPicked3 = playerActions.Get(playerActions.Count - 1).picked;
                patternCount["PicksDoubleBackNum"] += (pastPicked1 == pastPicked3 && pastPicked2 == userChoice) ? 1 : -1;
                if (pastPicked1 != pastPicked3 && pastPicked2 != userChoice && patternCount["PicksDoubleBackNum"] > 0)
                {
                    patternCount["PicksDoubleBackNum"] = 0;
                }
            }
        }

        private int picksFarthestNumPattern()
        {
            int toReturn = -1;

            if(patternCount["PicksFarthestNum"] > 2 && patternCount["PicksGivenNum"] < 0)
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
            if (playerActions.Count > 2)
            {
                if (lastChoiceToDeliver == inputs[0])
                {
                    patternCount["PicksFarthestNum"] += (userChoice == inputs[inputs.Length - 1]) ? 1 : -1;
                }
                else if (lastChoiceToDeliver == inputs[inputs.Length - 1])
                {
                    patternCount["PicksFarthestNum"] += (userChoice == inputs[0]) ? 1 : -1;
                }
            }
        }

        private int picksSuccesionWrapNumPattern()
        {
           int toReturn = -1;
            if(patternCount["PicksSuccesionWrapNum"] > 0 && patternCount["PicksGivenNum"] < 0)
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
            if (playerActions.Count >= checkAt)
            {
                bool pickedPlusOne = true;
                for (int i = checkAt; i > 0 && pickedPlusOne; i--)
                {
                    PlayerData current = playerActions.Get(playerActions.Count - i);
                    int shouldBe = (current.delivered + 1);
                    shouldBe = shouldBe <= inputs[inputs.Length-1] ? shouldBe : inputs[0];
                    pickedPlusOne = current.picked == shouldBe;
                }
                patternCount["PicksSuccesionWrapNum"] += pickedPlusOne? 1 : -1;
            }
        }

        private int picksReccesionWrapNumPattern()
        {
            int toReturn = -1;
            if (patternCount["PicksReccesionWrapNum"] > 0 && patternCount["PicksGivenNum"] < 0)
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
            if (playerActions.Count >= checkAt)
            {
                bool pickedPlusOne = true;
                for (int i = checkAt; i > 0 && pickedPlusOne; i--)
                {
                    PlayerData current = playerActions.Get(playerActions.Count - i);
                    int shouldBe = (current.delivered - 1);
                    shouldBe = shouldBe >= inputs[0] ? shouldBe :inputs[inputs.Length-1];
                    pickedPlusOne = current.picked == shouldBe;
                }
                patternCount["PicksReccesionWrapNum"] += pickedPlusOne ? 1 : -1;
            }
        }

        private void resetPatternIfOver(int threshHold, string key, int restartPositive = 1, int restartNegative = -1)
        {
            if (Math.Abs(patternCount[key]) >= threshHold)
            {
                patternCount[key] = patternCount[key] > 0 ? restartPositive : restartNegative;
            }
        }

        private int grabNextBestNumberDumb()
        {
            int toReturn = lastDesiredChoice;
            while (toReturn == lastDesiredChoice)
            {
                if (inputs.Length > 2)
                {
                    toReturn = new Random().Next(1, inputs.Length + 1);
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
                        if(highestCount < patternCount["Picks" + i] && i != toReturn)
                        {
                            highestIndex = i;
                            highestCount = patternCount["Picks" + i];
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
            addKeyToPatternCount("PicksGivenNum", 1);
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
             if (!patternCount.ContainsKey(key))
            {
                patternCount.Add(key, initialCount);
            }
        }

        //Get-ers
        public Dictionary<string, int> getPatternCount()
        {
            return patternCount;
        }
        public SizedList<PlayerData> getPlayerActions()
        {
            return playerActions;
        }
        public int getScore()
        {
            return score;
        }
        public int getTotalPossible()
        {
            return totalPossible;
        }
    }
    