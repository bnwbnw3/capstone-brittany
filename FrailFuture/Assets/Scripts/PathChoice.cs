using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

public class PathChoice : MonoBehaviour
{
    public float waitTimeTillClosePath= 0.25f;
    public Material openDoorMaterial;
    public Material closedDoorMaterial;
    public int pathNum;
    private bool passedThrough;

    void Awake()
    {
        this.renderer.material = openDoorMaterial;
        passedThrough = false;
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!passedThrough)
        {
            if (collider.isTrigger)
            {
                if (SoundManager.soundManager.getIsAiTalking())
                {
                    closingPath();
                }
            }
            else
            {
                if (!SoundManager.soundManager.getIsAiTalking())
                {
                    openingPath();
                }
            }
        }
    }

    public void OnTriggerExit(Collider c)
    {
        if (collider.isTrigger)
        {
            AILearningSim.AIsim.userPicked(pathNum);
            SoundManager.soundManager.playResponse();
            //StartCoroutine(closePath(waitTimeTillClosePath));
           // passedThrough = true;
        }
    }
    public void resetPath()
    {
        passedThrough = false;
        openingPath();
    }
    /*
    public IEnumerator closePath(float secondsToWait)
    {
        //play next audio for Naration/AI Thoughts
        AILearningSim.AIsim.userPicked(pathNum);
        SoundManager.soundManager.playResponse();
        yield return new WaitForSeconds(secondsToWait);
        closingPath();
    }*/

    void openingPath()
    {
        renderer.material = openDoorMaterial;
        collider.isTrigger = true;
    }

    void closingPath()
    {
        renderer.material = closedDoorMaterial;
        collider.isTrigger = false;
    }
}
