using UnityEngine;
using System.Collections;

public class HallwayDialogueDONATrigger : MonoBehaviour 
{
    public void OnTriggerEnter(Collider c)
    {
        if (collider.isTrigger && !SoundManager.soundManager.getAi_IsTalking())
        {
            SoundManager.soundManager.playDONADialogue();
        }
    }
}
