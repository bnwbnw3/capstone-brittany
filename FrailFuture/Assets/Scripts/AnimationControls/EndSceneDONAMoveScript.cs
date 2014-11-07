using UnityEngine;
using System.Collections;

public class EndSceneDONAMoveScript : MonoBehaviour {

    public float walkAmount = -0.02f;

    private Animator _animator;
    private Vector3 lastKnownRotation;
    private Vector3 lastKnownPosition;
    private Vector3 originalPos;
    private const float moveThreshold = 0.01f;
    private const float turnThreshold = 0.4f;
    private float originalY;
    private bool walkForward;
    private bool canWalk;

    public GameObject turnToObjOne;
    public GameObject turnToObjTwo;

    private bool IsWalking
    {
        get { return _animator.GetBool("IsWalking"); }
        set { _animator.SetBool("IsWalking", value); }
    }

    private bool IsStrafingLeft
    {
        get { return _animator.GetBool("IsStrafingLeft"); }
        set { _animator.SetBool("IsStrafingLeft", value); }
    }

    private bool IsStrafingRight
    {
        get { return _animator.GetBool("IsStrafingRight"); }
        set { _animator.SetBool("IsStrafingRight", value); }
    }

    private bool IsTurningLeft
    {
        get { return _animator.GetBool("IsTurningLeft"); }
        set { _animator.SetBool("IsTurningLeft", value); }
    }

    private bool IsTurningRight
    {
        get { return _animator.GetBool("IsTurningRight"); }
        set { _animator.SetBool("IsTurningRight", value); }
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        lastKnownRotation = this.gameObject.transform.rotation.eulerAngles;
        originalPos = this.gameObject.transform.position;
        originalY = originalPos.y;
        walkForward = false;
        canWalk = false;
    }

    void Update()
    {
        if (walkForward)
        {
            canWalk = true;
            Vector3 currentPos = this.gameObject.transform.position;
            this.gameObject.transform.position = new Vector3(currentPos.x + walkAmount, currentPos.y, currentPos.z);
        }
        checkWalking();
        checkTurning();
        if (!(IsTurningLeft || IsTurningRight || IsWalking))
        {
            hardResetY();
        }
        if (!canWalk)
        {
            this.gameObject.transform.position = originalPos;
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject == turnToObjOne)
        {
            LookAtTarget lookScript = this.gameObject.GetComponent<LookAtTarget>();
            lookScript.targetToLookAt = turnToObjTwo;
        }

        if (c.gameObject == turnToObjTwo)
        {
            Destroy(this.gameObject);
        }
    }

    void checkTurning()
    {
        Vector3 currentRotation = this.gameObject.transform.rotation.eulerAngles;
        float yDiff = lastKnownRotation.y - currentRotation.y;
        if (lastKnownRotation != currentRotation && Mathf.Abs(yDiff) > turnThreshold)
        {
            if (yDiff > 0)
            {
                IsTurningLeft = true;
            }
            else
            {
                IsTurningRight = true;
            }
        }
        else
        {
            IsTurningRight = false;
            IsTurningLeft = false;
        }
        lastKnownRotation = currentRotation;
    }

    void checkWalking()
    {
        Vector3 currentPos = this.gameObject.transform.position;
        float xDiff = lastKnownPosition.x - currentPos.x;
        float zDiff = lastKnownPosition.x - currentPos.x;
        if (canWalk && lastKnownPosition != currentPos && (Mathf.Abs(xDiff) > moveThreshold || Mathf.Abs(zDiff) > moveThreshold ))
        {
            if (xDiff > 0)
            {
                IsWalking = true;
            }
            else if (xDiff < 0)
            {
                IsTurningLeft = true;
            }
            if (zDiff > 0)
            {
                IsStrafingLeft = true;
            }
            else if (zDiff < 0)
            {
                IsStrafingRight = true;
            }
        }
        else
        {
            IsStrafingLeft = false;
            IsStrafingRight = false;
            IsTurningLeft = false;
            IsWalking = false;
        }
        lastKnownPosition = currentPos;
    }

    public void walkToExitControlRoom()
    {
        LookAtTarget lookScript = this.gameObject.GetComponent<LookAtTarget>();
        lookScript.targetToLookAt = turnToObjOne;
        walkForward = true;
    }

    void hardResetY()
    {
        Vector3 currentPos =  this.gameObject.transform.position ;
        this.gameObject.transform.position = new Vector3(currentPos.x, originalY, currentPos.z);
    }

    void deleteGameObject()
    {
        Destroy(this.gameObject);
    }
}
