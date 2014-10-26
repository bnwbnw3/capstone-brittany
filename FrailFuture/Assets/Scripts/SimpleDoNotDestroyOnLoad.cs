using UnityEngine;
using System.Collections;

public class SimpleDoNotDestroyOnLoad : MonoBehaviour {

    public static GameObject self;

    void Awake()
    {
        //Edit script Awake call through Edit->ProjectSettings->Script Order
        if (self == null)
        {
            DontDestroyOnLoad(gameObject);
            self = gameObject;
        }
        else if (self != gameObject)
        {
            Destroy(gameObject);
        }
    }
}
