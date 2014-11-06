using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour
{
    public int maxNumChoices;
    public int minNumChoices;
    public int maxRowsForGraph = 6;
    public bool useMaxRows;
    public int minNumPlayThroughsEditor = 3;

    //stuff that shouldn't be editable from editor
    public static GameControl control;
    public bool AbleToLoadGame { get; set; } 
    public AccessibleTransform StartingPlayerVars  { get; set; }
    public bool InvertY { get; set; }
    public bool InvertX { get; set; }
    public float MouseSensitivity { get; set; }
    public float BackgroundMusicVolume { get; set; }
    public float SoundEffectsVolume { get; set; }
    public string LastKnownFileName { get; set; }
    public bool WasLoaded { get; set; }
    public int MinNumPlayThroughs { get { return _minNumPlayThroughs; } }
    public int currentPlayThrough { get; set; } 
    public const string tempAutoSaveFileLocation = "TempSaveSpot3693";

    private AI ai;
    private int minRows = 4;
    private int numMazeEndings = 5;
    private string fileNameExtension;

    private int _minNumPlayThroughs;
    void Awake()
    {
        //Edit script Awake call through Edit->ProjectSettings->Script Order
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
            InvertY = false;
            InvertX = false;
            MouseSensitivity = 15F;
            BackgroundMusicVolume = 1.0f;
            SoundEffectsVolume = 0.25f;
            LastKnownFileName = "default";
            fileNameExtension = ".dat";
            WasLoaded = false;
            StartingPlayerVars = new AccessibleTransform();
            AbleToLoadGame = false;
            currentPlayThrough = 1;
            _minNumPlayThroughs = minNumPlayThroughsEditor;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }
    public void makeBeginnerAi()
    {
        WasLoaded = false;
        ai = new AI();
    }

    //HardCoded Maze
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
        fileName = fileName + fileNameExtension;
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
                + "\nGrade (User hit Ai perferred Endings) = " + ((float)allData.score.scoreOfPickingAiBestEnding / allData.score.totalEndings) * 100
                + "\n" + "Saved data to: " + Application.persistentDataPath + "/" + fileName);
        file.Close();
    }
    public void Load(string fileName)
    {
        fileName = fileName + fileNameExtension; 
        if (File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);

            AIData data = (AIData)bf.Deserialize(file);
            file.Close();
            ai = new AI(data.mazeInfo, new Neutrality(data.neutrality), data.brain, data.score, data.currentGraphIndex);
            WasLoaded = AbleToLoadGame = true;
        }
    }

    //Get-ers
    public AI getAi
    {
        get { return ai; }
    }
    public int getMinRows()
    {
        return minRows;
    }
    public int getNumMazeEndings()
    {
        return numMazeEndings;
    }
}