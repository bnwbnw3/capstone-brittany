using System;

[Serializable]
public class GameData
{
    public float neutrality;
    public int currentGraphIndex;
    public int gameLongevity;
    public int currentPlayThrough;
    public AIEndingsScore score;
    public Brain brain;
    public MazeGroup mazeInfo;
}