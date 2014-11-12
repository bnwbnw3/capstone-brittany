using UnityEngine;
using System.Collections;

public class EndRoomEvents : MonoBehaviour 
{
    public GameObject spawner;
    public GameObject player;

    public void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            GameControl.control.CurrentPlayThrough++;
            GameControl.control.EndNodeButtonPressed = true;

            transform.collider.isTrigger = false;
            //get the top parent, which holds neutrality, and grab neutrality
            NeutralityTypes type = (NeutralityTypes)transform.root.gameObject.GetComponentInChildren<GUINode>().EndNodeType;
            SoundManager.soundManager.playEndMaze(type);
            //reset BackEnd Ai
            GameControl.control.getAi.findNewPathIfReachedAnEnd();

            //make button disapear from view
            GameObject resetButton = GameObject.Find("ResetButton");
            Vector3 oldPos = resetButton.transform.position;
            resetButton.transform.position = new Vector3(oldPos.x, oldPos.y - 10, oldPos.z);

            //spawn end room stuffs
            float buffer = 3.0f;
            float scriptWaitTime = GameObject.Find("AiSpeaker").audio.clip.length + buffer;
            float timeTillDelete = scriptWaitTime * (0.5f); 
            GameObjectSpawner spawnScript = spawner.GetComponent<GameObjectSpawner>();
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
            StartCoroutine(FadeScreen(scriptWaitTime - 2));
        }
    }


    private IEnumerator ResetGame(float waitTime, Collider c)
    {
        yield return new WaitForSeconds(waitTime);
        transform.root.collider.isTrigger = false;
        GameObject mainCam = GameObject.Find("Main Camera");
        mainCam.camera.clearFlags = CameraClearFlags.Skybox;
        GameObject.Find("ResetButton").GetComponent<SaveOriginalPos>().reset();

        GameObject player = GameObject.Find("Player");
        player.transform.position = TeleportPlayerToOriginOnCollide.getOrigin();
        player.transform.localScale = GameControl.control.StartingPlayerVars.scale;
        player.transform.eulerAngles = GameControl.control.StartingPlayerVars.eulerAngles;
        NodeManager.nodeManager.showNextRoom(); 
        transform.collider.isTrigger = true;
    }

    private IEnumerator FadeScreen(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ScreenFader.screenFader.makeSolid();
    }

    private IEnumerator waitAndDrop(float waitTime, Collider c)
    {
        yield return new WaitForSeconds(waitTime);
        GameObject resetButton = GameObject.Find("ResetButton");
        Vector3 oldPosRB = resetButton.transform.position;
        resetButton.transform.position = new Vector3(oldPosRB.x - 100, oldPosRB.y - 10, oldPosRB.z);
        GameObject player = GameObject.Find("Player");
        Vector3 oldPos = c.transform.position;
        player.transform.position = new Vector3(oldPos.x, oldPos.y - 2, oldPos.z);
    }
}
