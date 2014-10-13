using UnityEngine;
using System.Collections;

public class BeginMaze : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerExit(Collider c)
    {
        if (collider.isTrigger)
        {
            SoundManager.soundManager.playStartMaze();

        }
    }
}
