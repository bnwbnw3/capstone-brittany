﻿using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider c)
    {
       var MSM = c.gameObject.GetComponent<MovementSoundManager>();
       c.transform.position = MSM.startingPosition;
       NodeManager.nodeManager.resetAllNodes();
    }
}
