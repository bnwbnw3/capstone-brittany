﻿using UnityEngine;
using System.Collections;

public class DestroyAllThatTouchMe : MonoBehaviour {

    public void OnTriggerEnter(Collider c)
    {
        Destroy(c.gameObject);
    }
}
