using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour
{
    public int maxNumChoices = 3;
    public int minNumChoices = 2;
    public static GameControl control;
    //private Brain brain;
    private AI ai;
    private Graph maze;
    private Dictionary<NeutralityTypes, int> mazeEndIndexs;
    private bool gameReady = false;
    private bool aiReady = false;

    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
        makeStartUpAi();
        gameReady = false;
    }

    void Start()
    {
    }

    void makeStartUpAi()
    {
         SizedList<PlayerData> temp = new SizedList<PlayerData>(10);
        BrainData bd = new BrainData() { pastPatterns = new Dictionary<string, int>(), pastActions = temp };
        Brain brain = new Brain(bd);
        createMaze();
        Neutrality neutrality = new Neutrality();
        ai = new AI(maze, neutrality, brain, mazeEndIndexs);
        aiReady = true;
    }

    void createMaze()
    {
        testMazeGraph1();
    }

    void testMazeGraph1()
    {
        maze = new Graph(12);
        maze.addEdge(0, 1, 1);
        maze.addEdge(0, 2, 1);
        maze.addEdge(1, 3, 1);
        maze.addEdge(1, 4, 1);
        maze.addEdge(1, 5, 1);
        maze.addEdge(2, 5, 1);
        maze.addEdge(2, 6, 1);
        maze.addEdge(3, 7, 1);
        maze.addEdge(3, 8, 1);
        maze.addEdge(4, 8, 1);
        maze.addEdge(4, 9, 1);
        maze.addEdge(4, 10, 1);
        maze.addEdge(5, 9, 1);
        maze.addEdge(5, 10, 1);
        maze.addEdge(5, 11, 1);
        maze.addEdge(6, 10, 1);
        maze.addEdge(6, 11, 1);

        mazeEndIndexs = new Dictionary<NeutralityTypes, int>();
        mazeEndIndexs[NeutralityTypes.Evil] = 7;
        mazeEndIndexs[NeutralityTypes.Agitated] = 8;
        mazeEndIndexs[NeutralityTypes.Neutral] = 9;
        mazeEndIndexs[NeutralityTypes.Lovely] = 10;
        mazeEndIndexs[NeutralityTypes.Heavenly] = 11;
    }

    public void Save(string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + fileName);

        BrainData data = new BrainData();
        data.pastPatterns = ai.getBrain().getPatternCount();
        data.pastActions = ai.getBrain().getPlayerActions();
        data.score = ai.getBrain().getScore();
        data.totalPossible = ai.getBrain().getTotalPossible();

        AIData allData = new AIData();
        allData.brain = data;
        allData.neutrality = ai.getNeutrality();

        bf.Serialize(file, allData);
        Debug.Log("Stats: AI scored: " + data.score + "/" + data.totalPossible
                + "  Grade = " + ((float)data.score / data.totalPossible) * 100
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
            Debug.Log("Loaded data from: " + Application.persistentDataPath + "/" + fileName);
            Brain newBrain = new Brain(data.brain);
            ai = new AI(maze, new Neutrality(data.neutrality), new Brain(data.brain), mazeEndIndexs);
        }
    }

    public AI Ai
    {
        get { return ai; }
    }

    public bool GameReady
    {
        get { return gameReady; }
        set { gameReady = value; }
    }

    public bool AiReady
    {
        get { return aiReady; }
        set { makeStartUpAi(); }
    }
}
