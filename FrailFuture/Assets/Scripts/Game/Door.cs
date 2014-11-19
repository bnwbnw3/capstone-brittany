using UnityEngine;
using System.Collections;

public abstract class Door : MonoBehaviour 
{
    public Material openDoorMaterial;
    public Material closedDoorMaterial;
    protected bool passedThrough;

     void Awake()
    {
        this.renderer.material = openDoorMaterial;
        passedThrough = false;
        OnAwake();
    }
    protected virtual void OnAwake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!passedThrough)
        {
            if (collider.isTrigger)
            {
                renderer.material = openDoorMaterial;
                if (SoundManager.soundManager.getAi_IsTalking())
                {
                    closingNode();
                }
            }
            else
            {
                renderer.material = closedDoorMaterial;
                openingNode();
            }
        }

        renderer.material = closedDoorMaterial;
        OnUpdate();
    }
    protected virtual void OnUpdate() {}

    protected void openingNode()
    {
      if (!SoundManager.soundManager.getAi_IsTalking())
      {
        //renderer.material = openDoorMaterial;
        collider.isTrigger = true;
        OnOpeningNode();
      }
    }
    protected virtual void OnOpeningNode(){}
    
    protected void closingNode()
    {
        //renderer.material = closedDoorMaterial;
        collider.isTrigger = false;
        OnClosingNode();
    }
    protected virtual void OnClosingNode(){}
}