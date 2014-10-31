using UnityEngine;
using System.Collections;

public class RandomLerpMe : MonoBehaviour {
    public float timeBetweenLerps;
    public Vector3 pos1;
    public Vector3 pos2;
    private bool isGoingRight;
    private bool canMove;
    private float checkAt;
	// Update is called once per frame
    void Start()
    {
        checkAt = 0.01f;
        isGoingRight = true;
        canMove = true;
    }
	void Update () 
    {
        if (canMove)
        {
            if (isGoingRight)
            {
                transform.position = Vector3.Lerp(transform.position, pos2, Time.deltaTime);
            }
            else if (!isGoingRight)
            {
                transform.position = Vector3.Lerp(transform.position, pos1, Time.deltaTime);
            }
        }
        float diff1 = (transform.position.x - pos1.x) +(transform.position.z - pos1.z);
        float diff2 = (transform.position.x - pos2.x) + (transform.position.z - pos2.z);
        if (((diff1 <= checkAt) && (diff1 >= -checkAt) && !isGoingRight) || ((diff2 <= checkAt) && (diff2 >= -checkAt) && isGoingRight))
       {
           StartCoroutine(changePos(timeBetweenLerps));
       }
	}

    private IEnumerator changePos(float timeTillNextMove)
    {
        canMove = false;
        isGoingRight = !isGoingRight;
        yield return new WaitForSeconds(timeBetweenLerps);
        canMove = true;
    }
}
