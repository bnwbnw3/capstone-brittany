using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RailPointManager : MonoBehaviour
{
    public GameObject HolderOfMoveToScript;
    public GameObject HolderOfLookAtScript;
    public string TagNameToGrabRailPoints = "RailPoint";
    public float Delay = 0.0f;
    public bool loopPath = false;

    public bool AtEndRailPoint { get { return (currentIndex >= allRailPoints.Length); } }
    private int currentIndex;
    private GameObject point;
    private GameObject[] allRailPoints;
    private bool moving;

    // Use this for initialization
    void Start()
    {
        allRailPoints = GameObject.FindGameObjectsWithTag(TagNameToGrabRailPoints);
        allRailPoints = allRailPoints.OrderBy(n => getIndexFromRailPointName(n.name)).ToArray();
        currentIndex = 0;
        point = allRailPoints[currentIndex];
        HolderOfMoveToScript.gameObject.transform.position = point.gameObject.transform.position;
        currentIndex++;
        moving = false;
        GameObject nextLookAt = getNextLookAt();

        point = allRailPoints[currentIndex];
        HolderOfLookAtScript.GetComponent<LookAtTarget>().TargetToLookAt = nextLookAt;
        HolderOfMoveToScript.GetComponent<MoveToTarget>().targetToMoveTo = point;
        StartCoroutine(WaitTillGo(Delay));
    }

    // Update is called once per frame
    void Update()
    {
        updateCurrentIndex();
        updateMoveAbility();
        HolderOfLookAtScript.GetComponent<LookAtTarget>().CanUpdate = moving;
        HolderOfMoveToScript.GetComponent<MoveToTarget>().CanUpdate = moving;
    }

    private void updateCurrentIndex()
    {
        if (moving)
        {
            point = allRailPoints[currentIndex];
            RailPoint rp = point.GetComponent<RailPoint>();

            if (rp.HasBeenReached)
            {
                Debug.Log("Index reached: " + currentIndex);
                currentIndex++;
            }
        }
    }
    private void updateMoveAbility()
    {
        if (AtEndRailPoint)
        {
            if (loopPath)
            {
                currentIndex = 0;
            }
            else
            {
                moving = false;
            }
        }
        else
        {
            setUpNextPathTo();
        }
    }
    private void setUpNextPathTo()
    {
       GameObject nextLookAt = getNextLookAt();
       HolderOfLookAtScript.GetComponent<LookAtTarget>().TargetToLookAt = nextLookAt;
       HolderOfMoveToScript.GetComponent<MoveToTarget>().targetToMoveTo = point;
    }
    private GameObject getNextLookAt()
    {
        point = allRailPoints[currentIndex];
        RailPoint rp = point.GetComponent<RailPoint>();
        GameObject nextLookAt = rp.ToLookAtWhenInRouteToMe;
        if (nextLookAt == null)
        {
            int pointIndex = currentIndex;
            nextLookAt = allRailPoints[pointIndex];
        }
        return nextLookAt;
    }

    private IEnumerator WaitTillGo(float time)
    {
        yield return new WaitForSeconds(time);
        moving = true;
    }

    int getIndexFromRailPointName(string src)
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