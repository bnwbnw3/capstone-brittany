using UnityEngine;
using System.Collections;

public class HallwayDialogueDONATrigger : MonoBehaviour 
{
    public void OnTriggerEnter(Collider c)
    {
        if (collider.isTrigger && !SoundManager.soundManager.getIsAiTalking())
        {
            SoundManager.soundManager.playDONADialogue();
        }
    }
}
