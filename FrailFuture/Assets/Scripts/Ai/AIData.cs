using System;

[Serializable]
public class AIData
{
    public float neutrality;
    public int currentGraphIndex;
    public AIEndingsScore score;
    public Brain brain;
    public MazeGroup mazeInfo;
}