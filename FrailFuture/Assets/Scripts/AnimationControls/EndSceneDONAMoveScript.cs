using UnityEngine;
using System.Collections;

public class EndSceneDONAMoveScript : MonoBehaviour {

    private Animator _animator;
    private Vector3 lastKnownRotation;
    private const float moveThreshold = 0.4f;
    private float originalY;
    private bool IsWalking
    {
        get { return _animator.GetBool("IsWalking"); }
        set { _animator.SetBool("IsWalking", value); }
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
        originalY = this.gameObject.transform.position.y;
    }

    void Update()
    {
        checkTurning();
        if (!(IsTurningLeft || IsTurningRight || IsWalking))
        {
            hardResetY();
        }
    }

    void checkTurning()
    {
        Vector3 currentRotation = this.gameObject.transform.rotation.eulerAngles;
        float yDiff = lastKnownRotation.y - currentRotation.y;
        if (lastKnownRotation != currentRotation && Mathf.Abs(yDiff) > moveThreshold)
        {
            Vector3 oldPos = this.gameObject.transform.position;
            this.gameObject.transform.position = new Vector3(oldPos.x, 0, oldPos.y);
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

    void hardResetY()
    {
        Vector3 currentPos =  this.gameObject.transform.position ;
        this.gameObject.transform.position = new Vector3(currentPos.x, originalY, currentPos.z);
    }
}
