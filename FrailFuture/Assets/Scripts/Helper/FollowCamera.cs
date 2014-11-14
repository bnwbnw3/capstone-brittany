using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour 
{
    public Camera followThis;

	void Update () 
    {
        manualUpdateAll();
	}

    public void manualUpdateAll()
    {
        updatePos();
        updateClearFlags();
    }

    public void updatePos()
    {
        this.transform.position = followThis.transform.position;
        this.transform.rotation = followThis.transform.rotation;
        this.transform.localScale = followThis.transform.localScale;
    }

    public void updateClearFlags()
    {
        this.camera.clearFlags = followThis.clearFlags;
    }
}