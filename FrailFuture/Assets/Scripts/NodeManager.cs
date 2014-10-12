using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour 
{

    //0 = start node
    //last = very last end node
    public List<GUINode> AllNodes;
    public static NodeManager nodeManager;
	// Use this for initialization
    void Awake()
    {
        if (nodeManager == null)
        {
            DontDestroyOnLoad(gameObject);
            nodeManager = this;
            init();
        }
        else if (nodeManager != this)
        {
            Destroy(gameObject);
        }
    }

    void init()
    {
        for (int i = 0; i < AllNodes.Count; i++)
        {
            AllNodes[i].init();
        }
    }

	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void resetAllNodes()
    {
        for (int i = 0; i < AllNodes.Count; i++)
        {
            AllNodes[i].resetGUINode();
        }
    }
}
