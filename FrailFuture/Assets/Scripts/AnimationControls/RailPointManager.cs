using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RailPointManager : MonoBehaviour {

    public Camera camToMove;
    public int numOfRailPoints;
    public float delay = 0.0f;
    private const int startingIndex =1;
    private int currentIndex;
    private Vector3 camPos;
    private GameObject point; 
    
	// Use this for initialization
	void Start ()
    {
        currentIndex = startingIndex;
        point = GameObject.Find("RP" + currentIndex);
        camToMove.gameObject.transform.position = point.gameObject.transform.position;
        currentIndex = 2;
        camToMove.GetComponent<LookAtTarget>().CanUpdate = false;
        camToMove.GetComponent<MoveToTarget>().CanUpdate = false;
        GameObject nextLookAt = getNextLookAt();

        point = GameObject.Find("RP" + currentIndex);
        camToMove.GetComponent<LookAtTarget>().targetToLookAt = nextLookAt;
        camToMove.GetComponent<MoveToTarget>().targetToMoveTo = point;
        StartCoroutine(WaitTillGo(delay));
	}
	
	// Update is called once per frame
	void Update () 
    {
        camPos = camToMove.gameObject.transform.position;
        point = GameObject.Find("RP" + currentIndex);
        RailPoint rp = point.GetComponent<RailPoint>();
        if(rp.HasBeenReached)
        {
            currentIndex++;
            if (currentIndex < numOfRailPoints)
            {
                GameObject nextLookAt = getNextLookAt();
                camToMove.GetComponent<LookAtTarget>().targetToLookAt = nextLookAt;
                camToMove.GetComponent<MoveToTarget>().targetToMoveTo = point;
            }
            else
            {
                //Done Moving
                camToMove.GetComponent<LookAtTarget>().CanUpdate = false;
                camToMove.GetComponent<MoveToTarget>().CanUpdate = false;
            }
        }
	}

    private GameObject getNextLookAt()
    {
        point = GameObject.Find("RP" + currentIndex);
        RailPoint rp = point.GetComponent<RailPoint>();
        GameObject nextLookAt = rp.ToLookAtWhenInRouteToMe;
        if (nextLookAt == null)
        {
            int pointIndex = currentIndex >= numOfRailPoints ? 0 : currentIndex;
            nextLookAt = GameObject.Find("RP" + currentIndex);
        }
        return nextLookAt;
    }

    private IEnumerator WaitTillGo(float time)
    {
        yield return new WaitForSeconds(time);
        camToMove.GetComponent<LookAtTarget>().CanUpdate = true;
        camToMove.GetComponent<MoveToTarget>().CanUpdate = true;
    }
}
