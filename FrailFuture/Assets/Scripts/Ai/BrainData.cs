using System;
using System.Collections.Generic;

[Serializable]
public class BrainData
{
    public Dictionary<string, int> pastPatterns;
    public SizedList<PlayerData> pastActions;
    public int scoreOfPickingDesiredInput;
    public int totalPossible;
}