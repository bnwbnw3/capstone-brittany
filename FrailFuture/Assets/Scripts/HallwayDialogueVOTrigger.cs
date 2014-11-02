using UnityEngine;
using System.Collections;

public class HallwayDialogueVOTrigger : MonoBehaviour {

    private int lastVertexKnown;

    void Awake()
    {
        lastVertexKnown = -1;
        if (GameControl.control.wasLoaded)
        {
            string name = this.gameObject.name;
            if (name.CompareTo("VOIntroTrigger") == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void OnTriggerEnter(Collider c)
    {
        if (lastVertexKnown != GameControl.control.Ai.getCurrentGraphIndex() || NodeManager.nodeManager.justReset)
        {
            if (!SoundManager.soundManager.getIsAiTalking())
            {
                lastVertexKnown = GameControl.control.Ai.getCurrentGraphIndex();
                SoundManager.soundManager.playVODialogue();
            }
        }
    }
}
