using UnityEngine;
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
        if (c.tag == "Player")
        {
            AILearningSim.AIsim.userPicked(pathNum);
            SoundManager.soundManager.playResponse();
        }
    }
    public void resetPath()
    {
        passedThrough = false;
        openingPath();
    }

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
