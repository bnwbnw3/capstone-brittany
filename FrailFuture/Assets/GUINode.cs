using UnityEngine;
using System.Collections.Generic;

public class GUINode : MonoBehaviour {

    public List<GUINode> Edges_Connections;
    public List<GUINode> extraCloseNodesOnTrigger;
    public Material openDoorMaterial;
    public Material closedDoorMaterial;
    private int index;

    void Awake()
    {
        this.renderer.material = openDoorMaterial;
    }
	// Use this for initialization
	void Start () 
    {
        index = NodeManager.nodeManager.AllNodes.IndexOf(this);
        if (index == -1)
        {
            //make sure this node is in GUINode Manager. 
            //Also make sure this nodes children are added to it.
            throw new UnassignedReferenceException("Make sure this node is in Node Manager./nAlso make sure this nodes children are added to it.");
        }
        Debug.Log("Node" + getIndexFromNodeManager() + " edge Nodes: ");
        for (int i = 0; i < Edges_Connections.Count; i++)
        {
            Debug.Log("Node" + Edges_Connections[i].getIndexFromNodeManager());
        }
	}

    // Update is called once per frame
    void Update() 
    {
	}

    public void OnTriggerEnter(Collider c)
    {
        if (collider.isTrigger)
        {
            renderer.material = closedDoorMaterial;
            collider.isTrigger = false;
            
        }
    }
    public int getIndexFromNodeManager()
    {
        return index;
    }
    public void resetGUINode()
    {
        renderer.material = openDoorMaterial;
        collider.isTrigger = true;

        if (Edges_Connections.Count > 0)
        {
            this.BroadcastMessage("resetPath");
        }
    }
}
