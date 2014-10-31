using UnityEngine;
using System.Collections;

public class HallwayDialogueVOTrigger : MonoBehaviour {

    private int lastVertexKnown;

    void Awake()
    {
        lastVertexKnown = -1;
    }
    public void OnTriggerEnter(Collider c)
    {
        if (lastVertexKnown != GameControl.control.Ai.getCurrentGraphIndex())
        {
            if (!SoundManager.soundManager.getIsAiTalking())
            {
                lastVertexKnown = GameControl.control.Ai.getCurrentGraphIndex();
                SoundManager.soundManager.playVODialogue();
            }
        }
    }
}
