using System;
using System.Collections.Generic;

[Serializable]
public class MazeGroup
{
    public Graph maze;
    public Dictionary<NeutralityTypes, int> mazeEndIndexs;
}