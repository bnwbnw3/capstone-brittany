using UnityEngine;
using System.Collections;

public class LookAtTarget : MonoBehaviour 
{
    public GameObject TargetToLookAt;
    public float acceleration = 0.5f;
    public float maxSpeed = 2.0f;
    public float minSpeed = 0.01f;
    public bool RestrictUpDownRotation = true;
    public float TurnThreshold = 0.001f;
    public bool CanUpdate { get; set; }
    private float Speed = 0;
    void Awake()
    {
        CanUpdate = true;
    }

	void Update () 
    {
        if (CanUpdate && TargetToLookAt != null)
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
            float compareAngle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(lookPos));
            var rotation = Quaternion.LookRotation(lookPos);
            if(compareAngle < 10)
            {
                Speed = Speed > minSpeed ? Speed - (acceleration * Time.deltaTime): minSpeed;
            }
            else
            {
                Speed = Speed < maxSpeed? Speed + (acceleration * Time.deltaTime): maxSpeed;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Speed);
        }
    }
}