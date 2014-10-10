using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PathChoice : MonoBehaviour
{
    public Material openDoorMaterial;
    public Material closedDoorMaterial;
    public int pathNum;

    void Awake()
    {
        this.renderer.material = openDoorMaterial;
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTriggerEnter(Collider c)
    {
        if (collider.isTrigger)
        {
            renderer.material = closedDoorMaterial;
            collider.isTrigger = false;
        }
    }
    public void resetPath()
    {
        renderer.material = openDoorMaterial;
        collider.isTrigger = true;
    }
}
