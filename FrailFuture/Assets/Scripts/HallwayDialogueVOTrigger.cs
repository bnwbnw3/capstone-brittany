using UnityEngine;
using System.Collections;

public class HallwayDialogueVOTrigger : MonoBehaviour {

    public void OnTriggerEnter(Collider c)
    {
        if (collider.isTrigger && !SoundManager.soundManager.getIsAiTalking())
        {
            SoundManager.soundManager.playVODialogue();
            collider.enabled = false;
        }
    }

    public void resetTrigger()
    {
        collider.enabled = true;
    }
}
