using UnityEngine;
using System.Collections;

public class HallwayDialogueVOTrigger : MonoBehaviour
{
    private static int lastVertexKnown;

    void Awake()
    {
        lastVertexKnown = -1;
        if (GameControl.control.WasLoaded)
        {
            destroyLargeTrigger();
        }
        else
        {
            string name = this.gameObject.name;
            if (name.CompareTo("VOTrigger") == 0)
            {
                this.enabled = false;
            }
        }
    }
    public void Update()
    {
        if (GameControl.control.JustReset)
        {
            destroyLargeTrigger();
        }
        if (lastVertexKnown != -1)
        {
            this.enabled = true;
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        playVODialogue();
    }
    public void playVODialogue()
    {
        if (lastVertexKnown != GameControl.control.Ai.getCurrentGraphIndex() || GameControl.control.JustReset)
        {
            if (!SoundManager.soundManager.getAi_IsTalking())
            {
                destroyLargeTrigger();
                lastVertexKnown = GameControl.control.Ai.getCurrentGraphIndex();
                SoundManager.soundManager.playVODialogue();
            }
        }
    }

    public void destroyLargeTrigger()
    {
        string name = this.gameObject.name;
            if (name.CompareTo("VOIntroTrigger") == 0)
            {
                Destroy(this.gameObject);
            }
    }
}