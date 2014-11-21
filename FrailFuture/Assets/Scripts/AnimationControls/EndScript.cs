using UnityEngine;
using System.Collections;

public class EndScript : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCam;
    public GameObject DONA;
    public Door TriggerToEnableMovement;
    public float delayTillAllowMovement= 1.5f;
    bool hasPlayedDialogue;
    bool hasToldDONAToMove;
    bool hasResetCam;

    void Start()
    {
        hasPlayedDialogue = false;
        hasToldDONAToMove = false;
        hasResetCam = false;
    }

    void Update()
    {
        if (!SoundManager.soundManager.getAi_IsTalking() && hasPlayedDialogue && !hasToldDONAToMove)
        {
            DONA.GetComponent<EndSceneDONAMoveScript>().walkToExitControlRoom();
            hasToldDONAToMove = true;
        }
        if (hasPlayedDialogue && hasToldDONAToMove && !hasResetCam)
        {
            if (TriggerToEnableMovement.TotalTriggerCount > 0)
            {
                StartCoroutine(waitToAllowPlayerMovement());
            }
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject == player && !hasPlayedDialogue)
        {
            SoundManager.soundManager.stopPlayerSounds();
            player.AddComponent<FirstPersonLookAtTarget>();
            player.GetComponent<FirstPersonLookAtTarget>().targetToLookAt = DONA;
            player.GetComponent<CharacterMotor>().enabled = false;
            player.GetComponent<FPSInputController>().enabled = false;
            player.GetComponent<MovementSoundManager>().enabled = false;
            player.GetComponent<MouseLook>().enabled = false;
            playerCam.GetComponent<MouseLook>().enabled = false;
            StartCoroutine(pauseToGiveAudio(3.5f));
        }
    }

    private IEnumerator pauseToGiveAudio(float waitTime)
    {
        if (GameControl.control.Ai != null)
        {
            SoundManager.soundManager.playOutro();
            yield return new WaitForSeconds(SoundManager.soundManager.totalOutroAudioTime);
        }
        else
        {
            yield return new WaitForSeconds(waitTime);
        }
        hasPlayedDialogue = true;
    }

    private IEnumerator waitToAllowPlayerMovement()
    {
        yield return new WaitForSeconds(delayTillAllowMovement);
        player.GetComponent<FirstPersonLookAtTarget>().enabled = false;

        player.GetComponent<CharacterMotor>().enabled = true;
        player.GetComponent<FPSInputController>().enabled = true;
        player.GetComponent<MovementSoundManager>().enabled = true;
        player.GetComponent<MouseLook>().enabled = true;
        playerCam.GetComponent<MouseLook>().enabled = true;

        hasResetCam = true;
        Destroy(this.gameObject);
    }
}