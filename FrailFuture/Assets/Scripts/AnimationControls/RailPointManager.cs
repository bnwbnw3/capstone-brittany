using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RailPointManager : MonoBehaviour {

    public Camera CameraToMove;
    public string TagNameToGrabRailPoints = "RailPoint";
    public float Delay = 0.0f;
    private const int startingIndex =0;
    private int currentIndex;
    private Vector3 camPos;
    private GameObject point;
    private GameObject[] allRailPoints;
    
	// Use this for initialization
	void Start ()
    {
        allRailPoints = GameObject.FindGameObjectsWithTag(TagNameToGrabRailPoints);
        allRailPoints = allRailPoints.OrderBy(n => getIndexsFromRailPointNames(n.name)).ToArray();
        currentIndex = startingIndex;
        point = allRailPoints[currentIndex];
        CameraToMove.gameObject.transform.position = point.gameObject.transform.position;
        currentIndex++;
        CameraToMove.GetComponent<LookAtTarget>().CanUpdate = false;
        CameraToMove.GetComponent<MoveToTarget>().CanUpdate = false;
        GameObject nextLookAt = getNextLookAt();

        point = allRailPoints[currentIndex];
        CameraToMove.GetComponent<LookAtTarget>().targetToLookAt = nextLookAt;
        CameraToMove.GetComponent<MoveToTarget>().targetToMoveTo = point;
        StartCoroutine(WaitTillGo(Delay));
	}
	
	// Update is called once per frame
	void Update () 
    {
        camPos = CameraToMove.gameObject.transform.position;
        point = allRailPoints[currentIndex];
        RailPoint rp = point.GetComponent<RailPoint>();
        if(rp.HasBeenReached)
        {
            currentIndex++;
            if (currentIndex < allRailPoints.Length)
            {
                GameObject nextLookAt = getNextLookAt();
                CameraToMove.GetComponent<LookAtTarget>().targetToLookAt = nextLookAt;
                CameraToMove.GetComponent<MoveToTarget>().targetToMoveTo = point;
            }
            else
            {
                //Done Moving
                CameraToMove.GetComponent<LookAtTarget>().CanUpdate = false;
                CameraToMove.GetComponent<MoveToTarget>().CanUpdate = false;
            }
        }
	}

    private GameObject getNextLookAt()
    {
        point = allRailPoints[currentIndex];
        RailPoint rp = point.GetComponent<RailPoint>();
        GameObject nextLookAt = rp.ToLookAtWhenInRouteToMe;
        if (nextLookAt == null)
        {
            int pointIndex = currentIndex >= allRailPoints.Length ? 0 : currentIndex;
            nextLookAt = allRailPoints[pointIndex];
        }
        return nextLookAt;
    }

    private IEnumerator WaitTillGo(float time)
    {
        yield return new WaitForSeconds(time);
        CameraToMove.GetComponent<LookAtTarget>().CanUpdate = true;
        CameraToMove.GetComponent<MoveToTarget>().CanUpdate = true;
    }

    int getIndexsFromRailPointNames(string src)
    {
        int ret = 0;
        int power = 1;
        for (int i = src.Length - 1; i >= 0; i--)
        {
            if ('0' <= src[i] && src[i] <= '9')
            {
                ret += ((int)src[i] - (int)'0') * power;
                power *= 10;
            }
        }
        return ret;
    }
}