using UnityEngine;
using System.Collections;

public class BeginMaze : MonoBehaviour {

    public void OnTriggerExit(Collider c)
    {
        if (collider.isTrigger && !SoundManager.soundManager.getIsAiTalking())
        {
            SoundManager.soundManager.playStartMaze();
        }
    }
}
