﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DummyNode : MonoBehaviour
{
    public Material openDoorMaterial;
    public Material closedDoorMaterial;
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
                    closingNode();
                }
            }
            else
            {
                if (!SoundManager.soundManager.getIsAiTalking())
                {
                    openingNode();
                }
            }
        }
    }
    void openingNode()
    {
        renderer.material = openDoorMaterial;
        collider.isTrigger = true;
    }

    void closingNode()
    {
        renderer.material = closedDoorMaterial;
        collider.isTrigger = false;
    }
}