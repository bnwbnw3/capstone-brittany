using UnityEngine;
using System.Collections;

public class TeleportToOrigin : MonoBehaviour {

    static private Vector3 origin;

    void Awake()
    {
        origin.x = origin.y = origin.z = 0;
    }
    public void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            c.transform.position = origin;
        }
    }
}
