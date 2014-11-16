using UnityEngine;
using System.Collections;

public class MoveToTarget : MonoBehaviour
{
    public GameObject targetToMoveTo;
    public float speed = 5.0f;
    public bool CanUpdate { get; set; }
    void Awake()
    {
        CanUpdate = true;
    }
    void Update()
    {
        if (CanUpdate)
        {
            moveToTarget(targetToMoveTo.transform.position);
        }
    }

    public void moveToTarget(Vector3 posToMoveTo)
    {
        transform.position = Vector3.MoveTowards(transform.position, posToMoveTo, Time.deltaTime * speed);
    }
}