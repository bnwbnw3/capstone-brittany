using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class Brain
    {
        public Dictionary<string, int> patternCount;
        int[] inputs;
        int lastDesiredChoice;
        int lastChoiceToDeliver;
        int score;

        public Brain()
        {
            patternCount = new Dictionary<string, int>()
            {
                {"PicksGivenNum", 1},
                {"TotalPickedAI", 0},
                {"TotalNotPickedAI", 0}
            };
            score = 0;
        }

        public void checkUserChoice(int userChoice)
        {
            if (userChoice == lastChoiceToDeliver)
            {
                patternCount["PicksGivenNum"] += 1;
            }
            else
            {
                patternCount["PicksGivenNum"] += -1;
            }

            if (userChoice == lastDesiredChoice)
            {
                score += 1;
            }
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
            int picksGivenNum = picksGivenNumPattern();
            return picksGivenNum;
        }

        private int picksGivenNumPattern()
        {
            int toReturn = -1;
            if (Math.Abs(patternCount["PicksGivenNum"]) >= 5)
            {
                patternCount["PicksGivenNum"] = patternCount["PicksGivenNum"] > 0 ? 1 : -1;
            }

            toReturn = lastDesiredChoice;
            if (patternCount["PicksGivenNum"] < 0)
            {
                while (toReturn == lastDesiredChoice)
                {
                    if (inputs.Length > 2)
                    {
                        toReturn = new Random().Next(1, inputs.Length + 1);
                    }
                    else
                    {
                        int index = lastDesiredChoice - 1 > 0 ? 0:1;
                        toReturn = inputs[index];
                    }
                }
                patternCount["TotalNotPickedAI"] += 1;
            }
            else
            {
                patternCount["TotalPickedAI"] += 1;
            }
            return toReturn;
        }

        private void checkForPatterns(string fileName)
        {
            //read in file and check for patterns with file instead of individual inputs
        }
    }
}
