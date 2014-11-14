using UnityEngine;
using System.Collections;

public class ForceFPSWalkForward : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject stopIfCollideWith;
    public float walkAmount = -0.02f;
    public bool walkForwardStarting = true;

    public bool WalkForward { get; set; }
    private bool currentlyWalking;
    private GameObject player;
    // Use this for initialization
    void Start()
    {
        player = this.gameObject;
        WalkForward = walkForwardStarting;
        currentlyWalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (WalkForward)
        {
            player.GetComponent<CharacterMotor>().enabled = false;
            player.GetComponent<FPSInputController>().enabled = false;
            player.GetComponent<MovementSoundManager>().enabled = false;
            player.GetComponent<MouseLook>().enabled = false;
            playerCamera.GetComponent<MouseLook>().enabled = false;
            currentlyWalking = true;
        }
        if (currentlyWalking)
        {
            Vector3 currentPos = this.gameObject.transform.position;
            this.gameObject.transform.position = new Vector3(currentPos.x + walkAmount, currentPos.y, currentPos.z);
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject == stopIfCollideWith)
        {
            currentlyWalking = false;
            WalkForward = false;
        }
    }
}
