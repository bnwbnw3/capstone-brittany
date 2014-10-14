using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class SoundManager : MonoBehaviour {
    public List<AudioClip> playerMovementSounds;
    public List<AudioClip> AiDirections;
    public List<AudioClip> PickedRightDoor;
    public List<AudioClip> PickedWrongDoor;
    public List<AudioClip> PickedWrong_DesiredDoor;
    public List<AudioClip> EngingsFromBestToWorst;
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

    void Update()
    {

    }

   public bool getIsAiTalking()
    {
        return GameObject.Find("AiSpeaker").audio.isPlaying;
    }

    //Player Movement Audio
	public void playWalkSound()
    {
       playAudio(playerMovementSounds[0], GameObject.Find("Player").audio, 0.20f, true);
    }
    public void stopPlayerSounds()
    {
        stopAudio(GameObject.Find("Player").audio);
    }
    public void playJumpSound()
    {
        playAudio(playerMovementSounds[1], GameObject.Find("Player").audio, 0.25f, false);
    }
    public void playLandSound()
    {
        playAudio(playerMovementSounds[2], GameObject.Find("Player").audio, 0.25f, false);
    }


    //Ai Audio

    //Ai Diolauge
    public void playStartMaze()
    {
        if (GameControl.control.Ai.getLastPickedInfo() == null)
        {
            startMaze0();
        }
        else
        {
            startMazeRandom();
        }
    }
    private void startMaze0()
    {
        //play intro DONA
        playAudio(AiDialogue[0], GameObject.Find("AiSpeaker").audio);
    }
    private void startMazeRandom()
    {
        //play random fact
        int index = randMaker.Next(0,AiComments.Count);
        playAudio(AiComments[index], GameObject.Find("AiSpeaker").audio);
    }

    public void playEndMaze(NeutralityTypes neutralityOfEnding)
    {
        Tools.AssertFalse(neutralityOfEnding == NeutralityTypes.None);
        playAudio(EngingsFromBestToWorst[(int)neutralityOfEnding], GameObject.Find("AiSpeaker").audio);
    }

    //Ai Instructions
    public void playDirection(int doorToPick)
    {
        Tools.AssertFalse(doorToPick <= 0 && doorToPick > GameControl.control.maxNumChoices);
        playAudio(AiDirections[doorToPick - 1], GameObject.Find("AiSpeaker").audio);
    }

    public void playResponse()
    {
        PlayerData pd = GameControl.control.Ai.getLastPickedInfo();
        if (pd.picked == pd.delivered)
        {
            int index = randMaker.Next(1, 5);
            index = index % 2 == 0 ? 0 : 1;
            playAudio(PickedRightDoor[index], GameObject.Find("AiSpeaker").audio);
        }
        else
        {
            if (pd.picked == pd.desired)
            {
                int index = randMaker.Next(0, 3);
                playAudio(PickedWrong_DesiredDoor[index], GameObject.Find("AiSpeaker").audio);
            }
            else
            {
                int index = randMaker.Next(1, 5);
                index = index % 2 == 0 ? 0 : 1;
                playAudio(PickedWrongDoor[index], GameObject.Find("AiSpeaker").audio);
            }
        }
    }

    private void playAudio(AudioClip clip, AudioSource source, float volume = 1.0f, bool loopMe = false)
    {
        source.clip = clip;
        source.loop = loopMe;
        source.volume = volume;
        source.Play();
        if (!source.loop)
        {
            StartCoroutine(stopNon_LoopAudio(source));
        }
    }

    private void stopAudio(AudioSource source)
    {
        if (source.audio.isPlaying && source.audio.loop)
        {
            source.loop = false;
            source.volume = 0.0f;
            source.Stop();
        }
    }

    private IEnumerator stopNon_LoopAudio(AudioSource source)
    {
        yield return new WaitForSeconds(source.audio.clip.length);
        source.loop = false;
        source.volume = 0.0f;
        source.Stop();
    }
}
