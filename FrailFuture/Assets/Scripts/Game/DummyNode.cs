using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DummyNode : Door
{
    public void OnTriggerExit(Collider c)
    {
        if (collider.isTrigger && c.tag == "Player")
        {
            passedThrough = true;
            closingNode();
        }
    }

    public void resetDummyNode()
    {
        openingNode();
        passedThrough = false;
    }
}