using UnityEngine;
using System.Collections;

public class EndRoomSpawner : MonoBehaviour {
    public GameObject spawner;

    public void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            transform.collider.isTrigger = false;
            //get the top parent, which holds neutrality, and grab neutrality
            NeutralityTypes type = (NeutralityTypes)transform.root.gameObject.GetComponentInChildren<GUINode>().endNodeType;

            SoundManager.soundManager.playEndMaze(type);

            float timeTillDelete = 3; ;
            float scriptWaitTime = GameObject.Find("AiSpeaker").audio.clip.length + 5; // get from Ai next speech length is for end index script
            GameObjectSpawner spawnScript = spawner.GetComponent<GameObjectSpawner>();
            GameObject resetButton = GameObject.Find("ResetButton");
            Vector3 oldPos = resetButton.transform.position;
            resetButton.transform.position = new Vector3(oldPos.x, oldPos.y - 10, oldPos.z);
            if (type == NeutralityTypes.Heavenly)
            {
                spawnScript.spawnObject(new int[] { 4, 5, 6 }, timeTillDelete, 30, 0.05f);
            }
            else if (type == NeutralityTypes.Lovely)
            {
                spawnScript.spawnObject(new int[] { 0, 1, 2, 3 }, timeTillDelete, 30, 0.05f);
            }
            else if (type == NeutralityTypes.Neutral)
            {
                spawnScript.spawnObject(new int[] { 7, 8, 9, 10 }, timeTillDelete, 30, 0.05f);
            }
            if (type == NeutralityTypes.Agitated)
            {
                StartCoroutine(waitAndDrop(scriptWaitTime/2, c));
            }
            else if (type == NeutralityTypes.Evil)
            {
                GameObject mainCam = GameObject.Find("Main Camera");
                mainCam.camera.clearFlags = CameraClearFlags.Color;
                spawnScript.spawnObject(new int[] { 11 }, scriptWaitTime, 1, 0.05f);
            }
            StartCoroutine(ResetGame(scriptWaitTime, c));
        }
    }


    private IEnumerator ResetGame(float waitTime, Collider c)
    {
        yield return new WaitForSeconds(waitTime); 
        transform.root.collider.isTrigger = false;
        GameObject mainCam = GameObject.Find("Main Camera");
        mainCam.camera.clearFlags = CameraClearFlags.Skybox;
        GameObject.Find("ResetButton").GetComponent<SaveOriginalPos>().reset();
        
        NodeManager.nodeManager.showNextRoom(); 
        transform.collider.isTrigger = true;

        StartingTransform spv = GameControl.control.getPlayerStartingTransform();
        c.transform.position = spv.pos;
        c.transform.localScale = spv.scale;
        c.transform.eulerAngles = spv.eulerAngles;
    }

    private IEnumerator waitAndDrop(float waitTime, Collider c)
    {
        yield return new WaitForSeconds(waitTime);
        Vector3 oldPos = c.transform.position;
        c.transform.position = new Vector3(oldPos.x, oldPos.y - 2, oldPos.z);
    }
}
