using UnityEngine;
using System.Collections;

public class TeleportPlayerToOriginOnCollide : MonoBehaviour 
{
    public Vector3 editorStartingOrigin = new Vector3(1, 0.05f, 0);
    static private Vector3 origin;

    void Awake()
    {
        origin.x = editorStartingOrigin.x;
        origin.z = editorStartingOrigin.y;
        origin.y = editorStartingOrigin.z;
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
            c.transform.position = origin;
        }
    }

    static public Vector3 getOrigin()
    {
        return origin;
    }
}