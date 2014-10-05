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
    private Brain brain;
    private bool gameReady = false;

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
        gameReady = false;
    }

    void Start()
    {
        //start with empty set. Use load to get in past data
        SizedList<PlayerData> temp = new SizedList<PlayerData>(10);
        BrainData bd = new BrainData() { pastPatterns = new Dictionary<string, int>(), pastActions = temp };
        brain = new Brain(bd);
    }

    public void Save(string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + fileName);

        BrainData data = new BrainData();
        data.pastPatterns = brain.getPatternCount();
        data.pastActions = brain.getPlayerActions();
        data.score = brain.getScore();
        data.totalPossible = brain.getTotalPossible();

        bf.Serialize(file, data);
        Debug.Log("Stats: AI scored: " +data.score + "/" + data.totalPossible 
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

            BrainData data = (BrainData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Loaded data from: " + Application.persistentDataPath + "/" + fileName);
            brain = new Brain(data);
        }
    }

    public Brain AiBrain
    {
        get { return brain; }
    }

    public bool GameReady
    {
        get { return gameReady; }
        set { gameReady = value; }
    }
}
