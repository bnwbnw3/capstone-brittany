using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class EndRoomEvents : MonoBehaviour 
{
    public GameObject player;

    private GameObject resetButton;
    private GameObject instructionBox;
    private GameObject spawner;
    public void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            GameControl.control.CurrentPlayThrough++;
            GameControl.control.EndNodeButtonPressed = true;
            resetButton = findEndNodeScriptGameObjs("ResetButton");
            instructionBox = findEndNodeScriptGameObjs("PushMeNotCanvas");
            spawner = findEndNodeScriptGameObjs("EndNodeObjectSpawner");

            transform.collider.isTrigger = false;
            //get the top parent, which holds neutrality, and grab neutrality
            NeutralityTypes type = (NeutralityTypes)transform.root.gameObject.GetComponentInChildren<GUINode>().EndNodeType;
            SoundManager.soundManager.playEndMaze(type);
            //reset BackEnd Ai
            GameControl.control.getAi.findNewPathIfReachedAnEnd();

            //make button disapear from view
            Vector3 oldPos = resetButton.transform.position;
            resetButton.transform.position = new Vector3(oldPos.x, oldPos.y - 10, oldPos.z);
            //make button directions disapear from view
            oldPos = instructionBox.transform.position;
            instructionBox.transform.position = new Vector3(oldPos.x, oldPos.y - 100, oldPos.z);
            instructionBox.GetComponent<DialogueBox>().DisableMyText();

            float buffer = 3.0f;
            float scriptWaitTime = GameObject.Find("AiSpeaker").audio.clip.length + buffer;
            float timeTillDelete = scriptWaitTime * (0.5f);
            spawnEndRoomStuff(type,timeTillDelete, scriptWaitTime, c);
            StartCoroutine(ResetGame(scriptWaitTime, c));
            StartCoroutine(FadeScreen(scriptWaitTime - 2));
        }
    }

    private GameObject findEndNodeScriptGameObjs(string name)
    {
        GameObject[] sameNameObjs = GameObject.FindGameObjectsWithTag("EndNodeScriptObjs");
        float minDistance = -1; ;
        int indexToUse = -1;
        for(int index = 0; index < sameNameObjs.Length; index++)
        {
            if (sameNameObjs[index].name == name)
            {
                float distanceToPlayer = Math.Abs(sameNameObjs[index].transform.position.x - player.transform.position.x);
                if (indexToUse < 0 || distanceToPlayer < minDistance) 
                {
                    indexToUse = index;
                    minDistance = distanceToPlayer;
                }
            }
        }
        return sameNameObjs[indexToUse];
    }

    private void spawnEndRoomStuff(NeutralityTypes currentNutrality, float timeTillDelete, float totalScriptTime, Collider colliderToChange)
    {
        GameObjectSpawner spawnScript = spawner.GetComponent<GameObjectSpawner>();
        if (currentNutrality == NeutralityTypes.Heavenly)
        {
            spawnScript.spawnObject(new int[] { 4, 5, 6 }, timeTillDelete, 30, 0.05f);
        }
        else if (currentNutrality == NeutralityTypes.Lovely)
        {
            spawnScript.spawnObject(new int[] { 0, 1, 2, 3 }, timeTillDelete, 30, 0.05f);
        }
        else if (currentNutrality == NeutralityTypes.Neutral)
        {
            spawnScript.spawnObject(new int[] { 7, 8, 9, 10 }, timeTillDelete, 30, 0.05f);
        }
        if (currentNutrality == NeutralityTypes.Agitated)
        {
            StartCoroutine(waitAndDrop(totalScriptTime / 2, colliderToChange));
        }
        else if (currentNutrality == NeutralityTypes.Evil)
        {
            player.GetComponent<MovementSoundManager>().SetCameraFlags(CameraClearFlags.Color);
            spawnScript.spawnObject(new int[] { 11 }, totalScriptTime, 1, 0.05f);
        }
    }

    private IEnumerator ResetGame(float waitTime, Collider c)
    {
        yield return new WaitForSeconds(waitTime);
        transform.root.collider.isTrigger = false;
        player.GetComponent<MovementSoundManager>().SetCameraFlags(CameraClearFlags.Skybox);
        resetButton.GetComponent<SaveOriginalPos>().reset();
        instructionBox.GetComponent<SaveOriginalPos>().reset();

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
        Vector3 oldPos = resetButton.transform.position;
        resetButton.transform.position = new Vector3(oldPos.x - 100, oldPos.y - 10, oldPos.z);

        oldPos = instructionBox.transform.position;
        instructionBox.transform.position = new Vector3(oldPos.x - 100, oldPos.y - 100, oldPos.z);

         oldPos = c.transform.position;
        player.transform.position = new Vector3(oldPos.x, oldPos.y - 2, oldPos.z);
    }
}
