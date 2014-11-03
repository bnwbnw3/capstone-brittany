using UnityEngine;
using System.Collections;

public class HallwayDialogueVOTrigger : MonoBehaviour {

    private static int lastVertexKnown;

    void Awake()
    {
        lastVertexKnown = -1;
        if (GameControl.control.wasLoaded)
        {
            destroyLargeTrigger();
        }
        else
        {
            string name = this.gameObject.name;
            if (name.CompareTo("VOTrigger") == 0)
            {
                this.enabled = false;
            }
        }
    }
    public void Update()
    {
        if (NodeManager.nodeManager.justReset)
        {
            destroyLargeTrigger();
        }
        if (lastVertexKnown != -1)
        {
            this.enabled = true;
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        if (lastVertexKnown != GameControl.control.Ai.getCurrentGraphIndex() || NodeManager.nodeManager.justReset)
        {
            if (!SoundManager.soundManager.getIsAiTalking())
            {
                destroyLargeTrigger();
                lastVertexKnown = GameControl.control.Ai.getCurrentGraphIndex();
                SoundManager.soundManager.playVODialogue();
            }
        }
    }

    public void destroyLargeTrigger()
    {
        string name = this.gameObject.name;
            if (name.CompareTo("VOIntroTrigger") == 0)
            {
                Destroy(this.gameObject);
            }
    }
}
