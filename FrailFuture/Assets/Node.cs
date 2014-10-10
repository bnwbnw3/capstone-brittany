using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    public List<Node> Edges_Connections;
    public List<Node> extraCloseNodesOnTrigger;
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
            //make sure this node is in Node Manager. 
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
    public void reset()
    {
        renderer.material = openDoorMaterial;
        collider.isTrigger = true;

        this.BroadcastMessage("reset");
    }
}


public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position,SerializedProperty property,GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}