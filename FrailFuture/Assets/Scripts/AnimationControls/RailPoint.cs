using UnityEngine;
using System.Collections;

public class RailPoint : MonoBehaviour
{
    public GameObject ToLookAtWhenInRouteToMe;
    public bool HasBeenReached { get; private set; }

	// Use this for initialization
	void Start () {
        HasBeenReached = false; 

        renderer.enabled = false;
        if (ToLookAtWhenInRouteToMe != null)
        {
            ToLookAtWhenInRouteToMe.renderer.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        HasBeenReached = true;
    }

    void OnTriggerExit(Collider c)
    {
        HasBeenReached = false;
    }
}
