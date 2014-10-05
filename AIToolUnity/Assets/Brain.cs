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
        public List<PlayerData> pastActions;
    }

    public class Brain
    {
        public Dictionary<string, int> patternCount;
        public List<PlayerData> playerActions;
        int[] inputs;
        int lastDesiredChoice;
        int lastChoiceToDeliver;
        int score;

        public Brain(BrainData pastHistory)
        {
            patternCount = pastHistory.pastPatterns;
            playerActions = pastHistory.pastActions;
            checkForKeys();
            score = 0;
        }

        public void checkUserChoice(int userChoice)
        {
            picksGivenNumCheck(userChoice);
            picksSpecificNumCheck(userChoice);

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
            int final = -1;
            int picksGivenNumR = picksGivenNumPattern();
            int picksSpecificNumR = picksSpecificNumPattern();
            final = picksSpecificNumR >= 1 ? picksSpecificNumR : picksGivenNumR;
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
                toReturn = playerActions[playerActions.Count - 1].picked;
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
            resetPatternIfOver(6, "PicksSpecificNum");
            return toReturn;
        }
        private void picksSpecificNumCheck(int userChoice)
        {
            if(playerActions.Count >= 1)
            {
                patternCount["PicksSpecificNum"] += (userChoice == playerActions[playerActions.Count - 1].picked) ? 1: -1;
            }
            patternCount["Picks" + userChoice] += 1;
        }

        private void resetPatternIfOver(int threshHold, string key)
        {
            if (Math.Abs(patternCount[key]) >= threshHold)
            {
                patternCount[key] = patternCount[key] > 0 ? 1 : -1;
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
            if (inputs.Length < 2)
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
            for (int i = 0; i < GameControl.control.maxNumChoices; i++)
            {
                addKeyToPatternCount("Picks" + (i + 1));
            }

        }

        //adds if doesn't exist
        private void addKeyToPatternCount(string key, int initialCount = 0)
        {
             if (!patternCount.ContainsKey(key))
            {
                patternCount.Add(key, initialCount);
            }
        }
    }