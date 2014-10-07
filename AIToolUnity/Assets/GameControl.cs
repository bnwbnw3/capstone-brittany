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
        //testMazeGraph1();
        //testMazeGraph2();
        testMazeGraph3();
    }

    void testMazeGraph1()
    {
        maze = new Graph(12);
        maze.addEdge(0, 1);
        maze.addEdge(0, 2);
        maze.addEdge(1, 3);
        maze.addEdge(1, 4);
        maze.addEdge(1, 5);
        maze.addEdge(2, 5);
        maze.addEdge(2, 6);
        maze.addEdge(3, 7);
        maze.addEdge(3, 8);
        maze.addEdge(4, 8);
        maze.addEdge(4, 9);
        maze.addEdge(4, 10);
        maze.addEdge(5, 9);
        maze.addEdge(5, 10);
        maze.addEdge(5, 11);
        maze.addEdge(6, 10);
        maze.addEdge(6, 11);

        mazeEndIndexs = new Dictionary<NeutralityTypes, int>();
        mazeEndIndexs[NeutralityTypes.Evil] = 7;
        mazeEndIndexs[NeutralityTypes.Agitated] = 8;
        mazeEndIndexs[NeutralityTypes.Neutral] = 9;
        mazeEndIndexs[NeutralityTypes.Lovely] = 10;
        mazeEndIndexs[NeutralityTypes.Heavenly] = 11;
    }
    void testMazeGraph2()
    {
        maze = new Graph(25);
        maze.addEdge(0, 1);
        maze.addEdge(0, 2);
        maze.addEdge(1, 3);
        maze.addEdge(1, 4);
        maze.addEdge(2, 5);
        maze.addEdge(2, 6);
        maze.addEdge(3, 7);
        maze.addEdge(3, 8);
        maze.addEdge(4, 9);
        maze.addEdge(4, 10);
        maze.addEdge(5, 8);
        maze.addEdge(5, 10);
        maze.addEdge(5, 11);
        maze.addEdge(6, 11);
        maze.addEdge(6, 12);
        maze.addEdge(7, 13);
        maze.addEdge(7, 14);
        maze.addEdge(7, 15);
        maze.addEdge(8, 15);
        maze.addEdge(8, 16);
        maze.addEdge(9, 16);
        maze.addEdge(9, 19);
        maze.addEdge(10, 17);
        maze.addEdge(10, 18);
        maze.addEdge(11, 17);
        maze.addEdge(11, 18);
        maze.addEdge(12, 18);
        maze.addEdge(12, 19);
        maze.addEdge(13, 20);
        maze.addEdge(13, 21);
        maze.addEdge(14, 20);
        maze.addEdge(14, 22);
        maze.addEdge(15, 21);
        maze.addEdge(15, 22);
        maze.addEdge(16, 20);
        maze.addEdge(16, 23);
        maze.addEdge(17, 21);
        maze.addEdge(17, 24);
        maze.addEdge(18, 23);
        maze.addEdge(18, 24);
        maze.addEdge(19, 22);
        maze.addEdge(19, 24);

        mazeEndIndexs = new Dictionary<NeutralityTypes, int>();
        mazeEndIndexs[NeutralityTypes.Evil] = 20;
        mazeEndIndexs[NeutralityTypes.Agitated] = 21;
        mazeEndIndexs[NeutralityTypes.Neutral] = 22;
        mazeEndIndexs[NeutralityTypes.Lovely] = 23;
        mazeEndIndexs[NeutralityTypes.Heavenly] = 24;
    }
    void testMazeGraph3()
    {
        maze = new Graph(19);
        maze.addEdge(0, 1);
        maze.addEdge(0, 2);
        maze.addEdge(0, 3);
        maze.addEdge(1, 4);
        maze.addEdge(1, 5);
        maze.addEdge(2, 6);
        maze.addEdge(2, 7);
        maze.addEdge(3, 7);
        maze.addEdge(3, 8);
        maze.addEdge(4, 9);
        maze.addEdge(4, 10);
        maze.addEdge(4, 11);
        maze.addEdge(5, 10);
        maze.addEdge(5, 11);
        maze.addEdge(5, 12);
        maze.addEdge(6, 9);
        maze.addEdge(6, 11);
        maze.addEdge(6, 12);
        maze.addEdge(7, 10);
        maze.addEdge(7, 12);
        maze.addEdge(7, 13);
        maze.addEdge(8, 9);
        maze.addEdge(8, 11);
        maze.addEdge(8, 13);
        maze.addEdge(9, 14);
        maze.addEdge(9, 17);
        maze.addEdge(10, 15);
        maze.addEdge(10, 16);
        maze.addEdge(11, 15);
        maze.addEdge(11, 18);
        maze.addEdge(12, 16);
        maze.addEdge(12, 18);
        maze.addEdge(13, 14);
        maze.addEdge(13, 16);

        mazeEndIndexs = new Dictionary<NeutralityTypes, int>();
        mazeEndIndexs[NeutralityTypes.Evil] = 14;
        mazeEndIndexs[NeutralityTypes.Agitated] = 15;
        mazeEndIndexs[NeutralityTypes.Neutral] = 16;
        mazeEndIndexs[NeutralityTypes.Lovely] = 17;
        mazeEndIndexs[NeutralityTypes.Heavenly] = 18;
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
