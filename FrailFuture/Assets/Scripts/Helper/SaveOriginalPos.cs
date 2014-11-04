using UnityEngine;
using System.Collections;

public class SaveOriginalPos : MonoBehaviour
{
    AccessibleTransform original;
    void Awake()
    {
        original = new AccessibleTransform();
        original.pos = transform.position;
        original.scale = transform.localScale;
        original.eulerAngles = transform.eulerAngles;
    }

    public void reset()
    {
        transform.position = original.pos;
        transform.localScale = original.scale;
        transform.eulerAngles = original.eulerAngles;
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "World")
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
    }
}