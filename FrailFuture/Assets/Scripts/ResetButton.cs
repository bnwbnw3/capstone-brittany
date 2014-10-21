using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

    public void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            StartingTransform spv = GameControl.control.getPlayerStartingTransform();
            c.transform.position = spv.pos;
            c.transform.localScale = spv.scale;
            c.transform.eulerAngles = spv.eulerAngles;

            NodeManager.nodeManager.showNextRoom();
        }
    }
}
