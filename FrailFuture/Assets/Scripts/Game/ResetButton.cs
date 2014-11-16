using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour
{
    public void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            c.transform.position = GameControl.control.StartingPlayerVars.pos;
            c.transform.localScale = GameControl.control.StartingPlayerVars.scale;
            c.transform.eulerAngles = GameControl.control.StartingPlayerVars.eulerAngles;

            NodeManager.nodeManager.showNextRoom();
        }
    }
}