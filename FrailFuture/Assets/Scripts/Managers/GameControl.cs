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
    public bool JustReset { get; set; }
    public bool EndNodeButtonPressed { get; set; }

    public bool WasLoaded { get; set; }
    public const string tempAutoSaveFileLocation = "TempSaveSpot3693";

    //Game Longevity
    public int GameLongevity
    {
        get { return _gameLongevity; }
        set
        {
            _gameLongevity = value;
            _gameLongevity = _gameLongevity < 1 ? 1 : _gameLongevity;
            _gameLongevity = _gameLongevity > 3 ? 3 : _gameLongevity;

            if (_gameLongevity == 1)
            {
                _numOfHallways = 1;
                _maxNumRows = _minNumRows;
                _minNumPlayThroughs = 2;
            }
            if (_gameLongevity == 2)
            {
                _numOfHallways = 2;
                _maxNumRows = _minNumRows + 1;// _minNumRows / 2;
                _minNumPlayThroughs = 2;// 3;
            }
            if (_gameLongevity == 3)
            {
                _numOfHallways = 3;
                _maxNumRows = _minNumRows + 2;// _minNumRows;
                _minNumPlayThroughs = 3;// 4;
            }
        }
    }
    //not to be changed w/ options
    public int NumberOfHallwaySections { get { return _numOfHallways; } }
    public int MaxNumberOfRows { get { return _maxNumRows; } }
    public int MinNumPlayThroughs { get { return _minNumPlayThroughs; } }
    public int CurrentPlayThrough { get; set; }

    private AI ai;
    private int _gameLongevity;
    private int _numOfHallways;
    private int _maxNumRows;
    private int _minNumPlayThroughs;
    private const int _minNumRows = 4;
    private const int numMazeEndings = 5;
    private string fileNameExtension;

    void Awake()
    {
        //Edit script Awake call through Edit->ProjectSettings->Script Order
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);


            fileNameExtension = ".dat";

            SetDefaultOptionSettings();
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    private void SetDefaultOptionSettings()
    {
        control = this;
        InvertY = false;
        InvertX = false;
        MouseSensitivity = 15F;
        BackgroundMusicVolume = 1.0f;
        SoundEffectsVolume = 0.25f;
        LastKnownFileName = "default";
        GameLongevity = 2;
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

        GameData allData = new GameData();
        allData.brain = ai.getBrain();
        allData.neutrality = ai.getNeutralityValue();
        allData.mazeInfo = ai.getMazeInfo();
        allData.currentGraphIndex = ai.getCurrentGraphIndex();
        allData.score = ai.getAIEndingsScore();
        allData.currentPlayThrough = CurrentPlayThrough;
        allData.gameLongevity = GameLongevity;
        allData.justReset = JustReset;
        allData.endNodeButtonPressed = EndNodeButtonPressed;

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

            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            CurrentPlayThrough = data.currentPlayThrough;
            GameLongevity = data.gameLongevity;
            JustReset = data.justReset;
            EndNodeButtonPressed = data.endNodeButtonPressed;

            ai = new AI(data.mazeInfo, new Neutrality(data.neutrality), data.brain, data.score, data.currentGraphIndex);
            WasLoaded = AbleToLoadGame = true;
        }
    }

    public void SetToDefaultGameValues()
    {
        if (CurrentPlayThrough > 0)
        {
            Save(GameControl.tempAutoSaveFileLocation);
        }
        WasLoaded = false;
        ai = new AI();
        StartingPlayerVars = new AccessibleTransform();
        AbleToLoadGame = false;
        CurrentPlayThrough = 0;
        JustReset = false;
        EndNodeButtonPressed = false;
        if (NodeManager.nodeManager != null)
        {
            NodeManager.nodeManager.setupAllHallways();
        }
    }

    //Get-ers
    public AI getAi
    {
        get { return ai; }
    }
    public int getMinRows()
    {
        return _minNumRows;
    }
    public int getNumMazeEndings()
    {
        return numMazeEndings;
    }
}