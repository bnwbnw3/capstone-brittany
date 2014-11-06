using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour 
{
    public GameObject targetToLookAt;
	// Update is called once per frame
	void Update () 
    {
        rotateToLookAtTarget(targetToLookAt.transform.position);
	}

    public void rotateToLookAtTarget(Vector3 posToLookAt)
    {
        int damping = 2;
        var lookPos = posToLookAt - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping); 
    }
}