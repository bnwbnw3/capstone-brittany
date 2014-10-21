using UnityEngine;
using System.Collections;

public class EndRoomSpawner : MonoBehaviour {
    public GameObject spawner;
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
            NeutralityTypes type = (NeutralityTypes)transform.root.gameObject.GetComponentInChildren<GUINode>().endNodeType;
            float timeTillDelete = 9; ;
            GameObjectSpawner spawnScript = spawner.GetComponent<GameObjectSpawner>();
            if (type == NeutralityTypes.Heavenly)
            {
                spawnScript.spawnObject(new int[] { 0, 1, 2, 3 }, timeTillDelete, 60, 0.05f);
            }
            else if (type == NeutralityTypes.Lovely)
            {
                spawnScript.spawnObject(new int[] { 4,5,6}, timeTillDelete, 60, 0.05f);
            }
            else if (type == NeutralityTypes.Neutral)
            {
                spawnScript.spawnObject(new int[] { 7, 8, 9, 10}, timeTillDelete, 30, 0.05f);
            }
            transform.collider.isTrigger = false;
            c.gameObject.GetComponent<CharacterMotor>().enabled = false;
            //Play script for end index, use script length + 1 for wait time

            float waitTime = 10;
            StartCoroutine(ResetGame(waitTime, c));
        }
    }

    private IEnumerator ResetGame(float waitTime, Collider c)
    {
        yield return new WaitForSeconds(waitTime); 
        StartingPlayerVariables spv = GameControl.control.getPlayerStartingTransform();
        c.transform.position = spv.pos;
        c.transform.localScale = spv.scale;
        c.transform.eulerAngles = spv.eulerAngles;
        NodeManager.nodeManager.showNextRoom(); 
        transform.collider.isTrigger = true;
        c.gameObject.GetComponent<CharacterMotor>().enabled = true;
    }
}
