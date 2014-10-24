using UnityEngine;
using System.Collections;

public class NodeTypeHolder : MonoBehaviour {

     private NeutralityTypes neutralityOfNode;
     private bool wasJustActivated;
    // Use this for initialization
    void Start()
    {
        neutralityOfNode = (NeutralityTypes)this.GetComponentInChildren<GUINode>().endNodeType;
        wasJustActivated = this.gameObject.activeInHierarchy;
    }
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject.activeInHierarchy != wasJustActivated && !this.gameObject.activeInHierarchy)
        {
           neutralityOfNode = (NeutralityTypes)this.GetComponentInChildren<GUINode>().endNodeType;
        }
        wasJustActivated = this.gameObject.activeInHierarchy;
	}
}
