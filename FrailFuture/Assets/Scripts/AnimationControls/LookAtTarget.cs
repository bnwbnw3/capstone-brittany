using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour 
{
    public GameObject TargetToLookAt;
    public float Speed = 1;
    public bool RestrictUpDownRotation = true;
    public float TurnThreshold = 0.001f;
    public bool CanUpdate {get;set;}
    void Awake()
    {
        CanUpdate = true;
    }

	void Update () 
    {
        if (CanUpdate)
        {
            rotateToLookAtTarget(TargetToLookAt.transform.position);
        }
	}

    public void rotateToLookAtTarget(Vector3 posToLookAt)
    {
        var lookPos = posToLookAt - transform.position;
        if (RestrictUpDownRotation)
        {
            lookPos.y = 0;
        }
        if (lookPos.sqrMagnitude > TurnThreshold)
        {
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Speed); 
        }
    }
}