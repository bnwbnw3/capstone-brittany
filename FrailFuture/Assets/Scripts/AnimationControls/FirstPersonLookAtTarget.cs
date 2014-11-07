using UnityEngine;
using System.Collections;

public class FirstPersonLookAtTarget : MonoBehaviour
{
    public GameObject targetToLookAt;
    public float radiusOfLookAt = 10.0f;
    // Update is called once per frame
    void Update()
    {
        rotateToLookAtTarget(targetToLookAt);
    }

    public void rotateToLookAtTarget(GameObject target)
    {
        float damping = 10.0f;
        GameObject player = this.gameObject;
        Vector3 objectPosition = target.transform.position;
        Vector3 playerPosition = player.transform.position;

        if (Vector3.Distance(objectPosition, playerPosition) < radiusOfLookAt)
        {
            //replacement for LookAt / LookRotation, forces gaze of player onto object
            Vector3 targetPoint = new Vector3(objectPosition.x, playerPosition.y, objectPosition.z) - playerPosition;
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint, Vector3.up);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * damping);
        }

    }
}