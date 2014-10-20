using UnityEngine;
using System.Collections;

public class TeleportToOrigin : MonoBehaviour {

    static private Vector3 origin;
    static private Vector3 eulerOrigin;

    void Awake()
    {
        origin.x = 1;
        origin.z = 0;
        origin.y = 0.05f;
    }
    public void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            NodeManager.nodeManager.showNextRoom();
            Vector3 Offset = this.transform.position - c.transform.position;
            Offset.z *= -1;
            Offset.x += 0.70f;
            Offset.y /= 10;
            origin = Offset;
            Debug.Log("offset of y: " + Offset.y);
            c.transform.position = origin;
        }
    }
}
