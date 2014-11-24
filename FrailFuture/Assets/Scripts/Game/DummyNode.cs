using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DummyNode : Door
{
    public void OnTriggerExit(Collider c)
    {
        TotalTriggerCount++;
        if (collider.isTrigger && c.tag == "Player")
        {
            passedThrough = true;
            closingNode();
        }
    }

    public void resetDummyNode()
    {
        collider.isTrigger = true;
        passedThrough = false;
        openingNode();
    }
}