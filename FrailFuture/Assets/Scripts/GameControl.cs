using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour
{
    public static GameControl control;
    public int maxNumChoices;
    public int minNumChoices;
    public int maxRowsForGraph = 6;
    public bool useMaxRows;
    private int minRows = 4;
    private int numMazeEndings = 5;

    static public bool ableToLoadGame;
    public bool invertY = false;
    public bool invertX = false;
    public float mouseSensitivity = 15F;
    public float backgroundMusicVolume = 1.0f;
    public float soundEffectsVolume =  0.25f;
    public bool wasLoaded;

    private AI ai;
    private bool gameReady;
    private bool aiReady;
    private StartingTransform _startingPlayerVars;
    private bool _playerStartTransNotSet;

    void Awake()
    {
        //Edit script Awake call through Edit->ProjectSettings->Script Order
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
        // makeBeginnerAi();
        gameReady = false;
        aiReady = false;
        wasLoaded = false;
        _startingPlayerVars = new StartingTransform();
        _playerStartTransNotSet = true;
        ableToLoadGame = false;
    }

    public void makeBeginnerAi()
    {
        wasLoaded = false;
         SizedList<PlayerData> temp = new SizedList<PlayerData>(10);
        BrainData bd = new BrainData() { pastPatterns = new Dictionary<string, int>(), pastActions = temp };
        Brain brain = new Brain(bd);
        System.Random random = new System.Random();
        Neutrality neutrality = new Neutrality(0);
        AIEndingsScore score = new AIEndingsScore();
        ai = new AI(createMaze(), neutrality, brain, score);
        aiReady = true;
    }

    MazeGroup createMaze()
    {
        int rowsToUse = 0;
        if(useMaxRows)
        {
            rowsToUse = maxRowsForGraph >= minRows ? maxRowsForGraph : minRows;
        }
        else
        {
            rowsToUse = new System.Random().Next(minRows, maxRowsForGraph+1);
        }
        return generateRandomGraph(rowsToUse);
        //testMazeGraph1();
    }

    MazeGroup generateRandomGraph(int maxRow)
    {
        Graph mazeToReturn;
        int rowReachsEndNodeCount = numMazeEndings - 1;
        //the row where increaseing nodes in rows stops. From this row to the next row
        //untill the max row decrease nodes. Highest Increase Point
        int Hip = rowReachsEndNodeCount + ((maxRow - rowReachsEndNodeCount) / 2);

        //find number of nodes for the graph
        int totalNodes = getTotalNodesFromRowAmount(maxRow, Hip);

        mazeToReturn = new Graph(totalNodes);
        
        //put in edges
        for (int row = 0; row < maxRow; row++)
        {
            if (row == 0)
            {
                mazeToReturn.addEdge(0, 1);
                mazeToReturn.addEdge(0, 2);
            }
            else
            {
                int[] fromNodes = getNodesInARow(row, Hip, maxRow);
                int[] toNodes = getNodesInARow(row + 1, Hip, maxRow);
                for (int j = fromNodes.Length-1; j >= 0 ; j--)
                {
                    int connections = new System.Random().Next(minNumChoices, maxNumChoices + 1);
                    int connectionCount = 0;
                    int currentConnectionPoint = 1;
                    int check = (j + currentConnectionPoint) - (connections - 1);
                    currentConnectionPoint += check >= 0 ? 0:(-1) *check;
                    if (fromNodes.Length == toNodes.Length && j == fromNodes.Length - 1)
                    {
                        currentConnectionPoint--;
                    }
                    while (connectionCount < connections)
                    {
                        mazeToReturn.addEdge(fromNodes[j], toNodes[j + currentConnectionPoint]);

                        currentConnectionPoint--;
                        connectionCount++;
                    }
                }
            }
        }

        //put in endings
        Dictionary<NeutralityTypes, int> endIndexs = new Dictionary<NeutralityTypes, int>();
        endIndexs[NeutralityTypes.Evil] = totalNodes - 5;
        endIndexs[NeutralityTypes.Agitated] = totalNodes - 4;
        endIndexs[NeutralityTypes.Neutral] = totalNodes - 3;
        endIndexs[NeutralityTypes.Lovely] = totalNodes - 2;
        endIndexs[NeutralityTypes.Heavenly] = totalNodes - 1;

        return new MazeGroup() { maze = mazeToReturn, mazeEndIndexs = endIndexs };
    }
    private int getTotalNodesFromRowAmount(int maxRow, int Hip)
    {
        int totalNodes = 0;
        int increaseAmount = 1;
        int decreaseAmount = 1;

        for (int rowLevel = 0; rowLevel <= maxRow; rowLevel++)
        {
            if (rowLevel <= Hip)
            {
                totalNodes += (rowLevel + increaseAmount);
            }
            else
            {
                int diffFromHip = rowLevel - Hip;
                totalNodes += (rowLevel + 1 - diffFromHip * 2);
                if (rowLevel == maxRow && rowLevel % 2 != 0)
                {
                    //if row is odd at the end the last row will not change from the row before it.
                    //re add in 1 to compensate.
                    totalNodes += 1;
                }
                decreaseAmount++;
            }
        }
        return totalNodes;
    }
    private int[] getNodesInARow(int rowNum, int Hip, int maxRow)
    {
        int beginOfRow = 0;
        int endOfRow = 0;
        if (rowNum <= Hip)
        {
            beginOfRow = Tools.SummationDownFrom(rowNum);
            endOfRow = beginOfRow + rowNum;
        }
        else
        {
            int diffFromHip = rowNum - Hip;
            beginOfRow = Tools.SummationDownFrom(Hip) + Tools.SummationChangeBy(Hip+1, diffFromHip, -1);
            endOfRow = beginOfRow + (rowNum - diffFromHip*2);

            if (rowNum == maxRow && rowNum % 2 != 0)
            {
                //if row is odd at the end the last row will not change from the row before it.
                //re add in 1 to compensate.
                endOfRow += 1;
            }
        }
        int[] nodesInRow = new int[(endOfRow - beginOfRow) + 1];
        //grab nodes in current row
        int count = 0;
        for (int j = beginOfRow; j <= endOfRow; j++)
        {
            nodesInRow[count++] = j;
        }
        return nodesInRow;
    }
    MazeGroup testMazeGraph1()
    {
        Graph theMaze = new Graph(12);
        theMaze.addEdge(0, 1);
        theMaze.addEdge(0, 2);
        theMaze.addEdge(1, 3);
        theMaze.addEdge(1, 4);
        theMaze.addEdge(1, 5);
        theMaze.addEdge(2, 5);
        theMaze.addEdge(2, 6);
        theMaze.addEdge(3, 7);
        theMaze.addEdge(3, 8);
        theMaze.addEdge(4, 8);
        theMaze.addEdge(4, 9);
        theMaze.addEdge(4, 10);
        theMaze.addEdge(5, 9);
        theMaze.addEdge(5, 10);
        theMaze.addEdge(5, 11);
        theMaze.addEdge(6, 10);
        theMaze.addEdge(6, 11);

        Dictionary<NeutralityTypes, int> endIndexs = new Dictionary<NeutralityTypes, int>();
        endIndexs[NeutralityTypes.Evil] = 7;
        endIndexs[NeutralityTypes.Agitated] = 8;
        endIndexs[NeutralityTypes.Neutral] = 9;
        endIndexs[NeutralityTypes.Lovely] = 10;
        endIndexs[NeutralityTypes.Heavenly] = 11;
        return new MazeGroup() { maze = theMaze, mazeEndIndexs = endIndexs };
    }

    public void Save(string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + fileName);


        AIData allData = new AIData();
        allData.brain = ai.getBrain();
        allData.neutrality = ai.getNeutralityValue();
        allData.mazeInfo = ai.getMazeInfo();
        allData.currentGraphIndex = ai.getCurrentGraphIndex();
        allData.score = ai.getAIEndingsScore();

        bf.Serialize(file, allData);
        Debug.Log(
                  "Stats: "
                + "\nAI input score: " + allData.brain.getScore() + "/" + allData.brain.getTotalPossible()
                + "\nAI's Picked Best Ending score: " + allData.score.scoreOfPickingAiBestEnding + "/" + allData.score.totalEndings
                + "\nAI's Picked Second Best Ending score: " + allData.score.scoreOfPickingAiSecondBestEnding + "/" + allData.score.totalEndings
                + "\nGrade = " + ((float)allData.score.scoreOfPickingAiBestEnding / allData.score.totalEndings) * 100
                + "\nCurrentVertex: " + allData.currentGraphIndex 
                + "\n" + "Saved data to: " + Application.persistentDataPath + "/" + fileName);
        file.Close();
    }

    public void Load(string fileName)
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);

            AIData data = (AIData)bf.Deserialize(file);
            file.Close();
            ai = new AI(data.mazeInfo, new Neutrality(data.neutrality), data.brain, data.score, data.currentGraphIndex);
            wasLoaded = ableToLoadGame = true;
        }
    }

    public AI Ai
    {
        get { return ai; }
    }

    public bool canSetPlayerStartingTransform
    {
        get { return _playerStartTransNotSet; }
    }
    public void setPlayerStartingTransform(Vector3 pos, Vector3 eulerAngles, Vector3 scale)
    {
        if (canSetPlayerStartingTransform)
        {
            _startingPlayerVars.pos = pos;
            _startingPlayerVars.eulerAngles = eulerAngles;
            _startingPlayerVars.scale = scale;
            _playerStartTransNotSet = false;
        }
    }
    public StartingTransform getPlayerStartingTransform()
    {
        return _startingPlayerVars;
    }

    public bool GameReady
    {
        get { return gameReady; }
        set { gameReady = value; }
    }

    public bool AiReady
    {
        get { return aiReady; }
    }
}

public class StartingTransform
{
    public Vector3 pos;
    public Vector3 eulerAngles;
    public Vector3 scale;
}
