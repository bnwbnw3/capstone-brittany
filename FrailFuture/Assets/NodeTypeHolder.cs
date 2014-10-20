using UnityEngine;
using System.Collections;

public class NodeTypeHolder : MonoBehaviour {

     [ReadOnly]public NeutralityTypes neutralityOfNode;
     private bool updateMe;
    // Use this for initialization
    void Start()
    {
        neutralityOfNode = (NeutralityTypes)this.GetComponentInChildren<GUINode>().endNodeType;
        updateMe = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject.activeInHierarchy && updateMe == true)
        {
            neutralityOfNode = (NeutralityTypes)this.GetComponentInChildren<GUINode>().endNodeType;
            updateMe = false;
        }
        else
        {
            updateMe = !this.gameObject.activeInHierarchy;
        }
	}
}
