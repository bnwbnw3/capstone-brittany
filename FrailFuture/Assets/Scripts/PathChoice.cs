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

    void Awake()
    {
        this.renderer.material = openDoorMaterial;
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTriggerExit(Collider c)
    {
        if (collider.isTrigger)
        {
            StartCoroutine(closePath(waitTimeTillClosePath));
        }
    }
    public void resetPath()
    {
        renderer.material = openDoorMaterial;
        collider.isTrigger = true;
    }

    public IEnumerator closePath(float secondsToWait)
    {
        //play next audio for Naration/AI Thoughts
        AILearningSim.AIsim.userPicked(pathNum);
        yield return new WaitForSeconds(secondsToWait); 
        renderer.material = closedDoorMaterial;
        collider.isTrigger = false;
    }
}
