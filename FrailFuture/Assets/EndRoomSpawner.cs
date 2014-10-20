using UnityEngine;
using System.Collections;

public class EndRoomSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
       
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            //get the top parent, which holds neutrality, and grab neutrality
            NeutralityTypes type = transform.root.gameObject.GetComponent<NodeTypeHolder>().neutralityOfNode;

            GameObjectSpawner spawner = GetComponentInChildren<GameObjectSpawner>();
            if (type == NeutralityTypes.Heavenly)
            {
                spawner.spawnObject(new int[] { 0, 1, 2, 3, 4, 5, 6 }, 20, 0.5f);
            }
            else if (type == NeutralityTypes.Lovely)
            {
                spawner.spawnObject(new int[] { 7,8,9 }, 10, 1.0f);
            }
            else if (type == NeutralityTypes.Neutral)
            {
                spawner.spawnObject(new int[] { 10,11,12,13 }, 8, 1.0f);
            }
            //Do this stuff after spawning items (good or bad) and playing Ai script.
            /*
            StartingPlayerVariables spv = GameControl.control.getPlayerStartingTransform();
            c.transform.position = spv.pos;
            c.transform.localScale = spv.scale;
            c.transform.eulerAngles = spv.eulerAngles;

            NodeManager.nodeManager.showNextRoom();*/
        }
    }
}
