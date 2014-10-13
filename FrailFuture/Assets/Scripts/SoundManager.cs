using UnityEngine;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour {
    public List<AudioClip> playerMovementSounds;
    public List<AudioClip> AiDirections;
    public List<AudioClip> PickedRightDoor;
    public List<AudioClip> PickedWrongDoor;
    public List<AudioClip> PickedWrong_DesiredDoor;
    public List<AudioClip> EngingsFromWorstToBest;
    public List<AudioClip> AiDialogue;
    public List<AudioClip> AiComments;

    private System.Random randMaker;
    //Do pattern stating later.
    //public Dictionary<string, List<AudioClip>> patterns;

    public static SoundManager soundManager;
    void Awake()
    {
        if (soundManager == null)
        {
            DontDestroyOnLoad(gameObject);
            soundManager = this;
            randMaker = new System.Random();
        }
        else if (soundManager != this)
        {
            Destroy(gameObject);
        }
    }

    //Player Movement Audio
	public void playWalkSound(AudioSource source)
    {
       playAudio(playerMovementSounds[0], source, 0.5f, true);
    }
    public void stopWalkSound(AudioSource source)
    {
        stopAudio(source);
    }


    //Ai Audio

    //Ai Diolauge
    public void playStartMaze(AudioSource source)
    {
        if (GameControl.control.Ai.getLastPickedInfo() == null)
        {
            startMaze0(source);
        }
        else
        {
            startMazeRandom(source);
        }
    }
    private void startMaze0(AudioSource source)
    {
        //play intro DONA
        playAudio(AiDialogue[0], source);
    }
    private void startMazeRandom(AudioSource source)
    {
        //play random fact
        int index = randMaker.Next(0,AiComments.Count);
        playAudio(AiComments[index], source);
    }

    public void playEndMaze(AudioSource source, NeutralityTypes neutralityOfEnding)
    {
        Tools.AssertFalse(neutralityOfEnding == NeutralityTypes.None);
        playAudio(EngingsFromWorstToBest[(int)neutralityOfEnding],source);
    }

    //Ai Instructions
    public void playDirection(AudioSource source, int doorToPick)
    {
        Tools.AssertFalse(doorToPick <= 0 && doorToPick > GameControl.control.maxNumChoices);
        playAudio(AiDirections[doorToPick - 1], source);
    }

    public void playResponse(AudioSource source)
    {
        PlayerData pd = GameControl.control.Ai.getLastPickedInfo();
        if (pd.picked == pd.delivered)
        {
            int index = randMaker.Next(1, 5);
            index = index % 2 == 0 ? 0 : 1;
            playAudio(PickedRightDoor[index], source);
        }
        else
        {
            if (pd.picked == pd.desired)
            {
                int index = randMaker.Next(1, 4);
                playAudio(PickedRightDoor[index], source);
            }
            else
            {
                int index = randMaker.Next(1, 5);
                index = index % 2 == 0 ? 0 : 1;
                playAudio(PickedWrongDoor[index], source);
            }
        }
    }

    private void playAudio(AudioClip clip, AudioSource source, float volume = 0.5f, bool loopMe = false)
    {
        source.clip = clip;
        source.loop = loopMe;
        source.volume = volume;
        source.Play();
    }

    private void stopAudio(AudioSource source)
    {
        source.loop = false;
        source.Stop();
    }
}
