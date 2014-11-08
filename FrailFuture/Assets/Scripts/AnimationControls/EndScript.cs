using UnityEngine;
using System.Collections;

public class EndScript : MonoBehaviour
{
    public GameObject player;
    public GameObject playerCam;
    public GameObject DONA;
    public GameObject endLookAt;
    bool hasPlayedDialogue;
    bool hasToldDONAToMove;
    bool hasResetCam;
    // Use this for initialization
    void Start()
    {
        hasPlayedDialogue = false;
        hasToldDONAToMove = false;
        hasResetCam = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!SoundManager.soundManager.getIsAiTalking() && hasPlayedDialogue && !hasToldDONAToMove)
        {
            DONA.GetComponent<EndSceneDONAMoveScript>().walkToExitControlRoom();
            hasToldDONAToMove = true;
        }
        if (hasPlayedDialogue && hasToldDONAToMove && !hasResetCam)
        {
            if (DONA == null)
            {
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
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject == player && !hasPlayedDialogue)
        {
            SoundManager.soundManager.stopPlayerSounds();
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
        SoundManager.soundManager.playOutro();
        yield return new WaitForSeconds(SoundManager.soundManager.totalOutroAudioTime);

        player.AddComponent<FirstPersonLookAtTarget>();

        player.GetComponent<FirstPersonLookAtTarget>().targetToLookAt = DONA;
        hasPlayedDialogue = true;
    }
}
