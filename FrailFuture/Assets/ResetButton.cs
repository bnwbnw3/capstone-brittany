using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            StartingPlayerVariables spv = GameControl.control.getPlayerStartingTransform();
            c.transform.position = spv.pos;
            c.transform.localScale = spv.scale;
            c.transform.eulerAngles = spv.eulerAngles;
            NodeManager.nodeManager.resetAllNodes();
        }
    }
}
