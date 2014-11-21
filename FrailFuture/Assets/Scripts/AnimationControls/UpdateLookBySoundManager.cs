using UnityEngine;
using System.Collections;

public class UpdateLookBySoundManager : MonoBehaviour
{

    public LookAtTarget scriptConnected;

    void Update()
    {
        scriptConnected.CanUpdate = SoundManager.soundManager.getAi_IsTalking(); ;
    }
}