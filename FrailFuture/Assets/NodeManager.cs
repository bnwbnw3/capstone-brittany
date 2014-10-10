using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour 
{

    //0 = start node
    //last = very last end node
    public List<Node> AllNodes;
    public static NodeManager nodeManager;
	// Use this for initialization
    void Awake()
    {
        if (nodeManager == null)
        {
            DontDestroyOnLoad(gameObject);
            nodeManager = this;
        }
        else if (nodeManager != this)
        {
            Destroy(gameObject);
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
            AllNodes[i].reset();
        }
    }
}
