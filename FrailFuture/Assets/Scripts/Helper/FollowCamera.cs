using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour 
{
    public Camera followThis;

	void Update () 
    {
        this.transform.position =   followThis.transform.position;
        this.transform.rotation =   followThis.transform.rotation;
        this.transform.localScale = followThis.transform.localScale;
	}
}