using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour 
{
    public GameObject targetToLookAt;
    public float speed = 2;
    public bool CanUpdate {get;set;}
    void Awake()
    {
        CanUpdate = true;
    }

	void Update () 
    {
        if (CanUpdate)
        {
            rotateToLookAtTarget(targetToLookAt.transform.position);
        }
	}

    public void rotateToLookAtTarget(Vector3 posToLookAt)
    {
        var lookPos = posToLookAt - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed); 
    }
}