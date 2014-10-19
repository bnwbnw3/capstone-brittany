using UnityEngine;
using System.Collections;

public class TeleportToOrigin : MonoBehaviour {

    static private Vector3 origin;
    static private Vector3 eulerOrigin;

    void Awake()
    {
        origin.x = origin.z = 0;
        origin.y = 0.05f;
        eulerOrigin.x = eulerOrigin.z = 0;
        eulerOrigin.y = 270;
    }
    public void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            NodeManager.nodeManager.showNextRoom();
            c.transform.position = origin;
            c.transform.eulerAngles = eulerOrigin;
        }
    }
}
