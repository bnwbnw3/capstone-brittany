using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour
{
    public static GameControl control;
    public Brain AIBrain;
    public bool gameReady;

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
        //this is where to load data if there is any
        Dictionary<string, int> basePatterns = new Dictionary<string, int>()
            {
                {"PicksGivenNum", 1},
                {"TotalPickedAI", 0},
                {"TotalNotPickedAI", 0},
                {"PicksSpecificNum",0},
                {"Picks1",0},
                {"Picks2",0},
                {"Picks3",0}
            };
        BrainData bd = new BrainData() { pastPatterns = basePatterns, pastActions = new List<PlayerData>() };
        AIBrain = new Brain(bd);
    }

    public void Save(string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + fileName);

        BrainData data = new BrainData();
        data.pastPatterns = AIBrain.patternCount;
        data.pastActions = AIBrain.playerActions;

        bf.Serialize(file, data);
        Debug.Log("Saved data to: " + Application.persistentDataPath + "/" + fileName);
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
            AIBrain = new Brain(data);
        }
    }
}
