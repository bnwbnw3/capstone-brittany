using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioListener gameListener;
    public List<AudioClip> playerMovementSounds;

    public List<AudioClip> AiDirectionsDoor1;
    public List<AudioClip> AiDirectionsDoor2;
    public List<AudioClip> AiDirectionsDoor3;

    public List<AudioClip> PickedRightDoor;
    public List<AudioClip> PickedWrongDoor;
    public List<AudioClip> PickedRightDoor_Desired;
    public List<AudioClip> PickedWrongDoor_Desired;
    public List<AudioClip> EngingsFromBestToWorst;

    public List<AudioClip> IntroAudio;
    public List<AudioClip> DONADialoguePos;
    public List<AudioClip> DONADialogueNeg;
    public List<AudioClip> RoomDialogue;
    public List<AudioClip> VODialogue;
    private int VO_DIndex = 0;

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
       playAudio(playerMovementSounds[0], GameObject.Find("Player").audio, 1.00f, true);
    }
    public void stopPlayerSounds()
    {
        stopAudio(GameObject.Find("Player").audio);
    }
    public void playJumpSound()
    {
        playAudio(playerMovementSounds[1], GameObject.Find("Player").audio, 1.00f, false);
    }
    public void playLandSound()
    {
        playAudio(playerMovementSounds[2], GameObject.Find("Player").audio, 1.00f, false);
    }


    //Ai Audio

    //Ai Dialogue

    public void playDONADialogue()
    {
        //play random dialogue from DONA based on neutrality
        if (GameControl.control.Ai.getCurrentGraphIndex() == 0 && GameControl.control.Ai.getLastPickedInfo() == null)
        {
            playDONAIntro(GameObject.Find("AiSpeaker").audio);
        }
        else if (GameControl.control.Ai.getNeutralityValue() >= 0)
        {
            int index = randMaker.Next(0,DONADialoguePos.Count);
            playAudio(DONADialoguePos[index], GameObject.Find("AiSpeaker").audio, 2.0f);
        }
        else
        {
            int index = randMaker.Next(0, DONADialogueNeg.Count);
            playAudio(DONADialogueNeg[index], GameObject.Find("AiSpeaker").audio, 2.0f);
        }
    }

    public void playVODialogue()
    {
        if (GameControl.control.Ai.getCurrentGraphIndex() == 0 && GameControl.control.Ai.getLastPickedInfo() == null)
        {
            StartCoroutine(playVOIntro(GameObject.Find("AiSpeaker").audio));
        }
        else
        {
            StartCoroutine(playVoDialogue(GameObject.Find("AiSpeaker").audio));
        }
    }

    public void playEndMaze(NeutralityTypes neutralityOfEnding)
    {
        Tools.AssertFalse(neutralityOfEnding == NeutralityTypes.None);
        playAudio(EngingsFromBestToWorst[(int)neutralityOfEnding], GameObject.Find("AiSpeaker").audio, 2.0f);
    }

    //Ai Instructions
    public void playDirection(int doorToPick)
    {
        Tools.AssertFalse(doorToPick <= 0 && doorToPick > GameControl.control.maxNumChoices);
        int AiNeutrality = (int)GameControl.control.Ai.getNeutralityState();
        if (doorToPick == 1)
        {
            playAudio(AiDirectionsDoor1[AiNeutrality], GameObject.Find("AiSpeaker").audio, 2.0f);
        }
        else if (doorToPick == 2)
        {
            playAudio(AiDirectionsDoor2[AiNeutrality], GameObject.Find("AiSpeaker").audio, 2.0f);
        }

        else if (doorToPick == 3)
        {
            playAudio(AiDirectionsDoor3[AiNeutrality], GameObject.Find("AiSpeaker").audio, 2.0f);
        }
    }

    public void playResponse()
    {
        PlayerData pd = GameControl.control.Ai.getLastPickedInfo();
        bool aiIsPos = GameControl.control.Ai.getNeutralityValue() >= 0;
        //grab random index based on having pos or neg neutrality
        int index = randMaker.Next(2, (PickedRightDoor_Desired.Count));
        if (index % 2 == 0 && aiIsPos)
        {
            index -= 1;
        }
        if (index % 2 != 0 && !aiIsPos)
        {
            index -= 1;
        }
        Debug.Log("isGood: " +aiIsPos + "--> Response index: " + index);
        if (pd.picked == pd.delivered)
        {
            if (pd.picked == pd.desired)
            {
                playAudio(PickedRightDoor_Desired[index], GameObject.Find("AiSpeaker").audio, 2.0f);
            }
            else
            {
                playAudio(PickedRightDoor[index], GameObject.Find("AiSpeaker").audio, 2.0f);
            }
        }
        else
        {
            if (pd.picked == pd.desired)
            {
                playAudio(PickedWrongDoor_Desired[index], GameObject.Find("AiSpeaker").audio, 2.0f);
            }
            else
            {
                playAudio(PickedWrongDoor[index], GameObject.Find("AiSpeaker").audio, 2.0f);
            }
        }
    }

    private void playAudio(AudioClip clip, AudioSource source, float volume = 1.0f, bool loopMe = false)
    {
        source.clip = clip;
        source.loop = loopMe;
        source.volume = volume*GameControl.control.soundEffectsVolume;
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

    private void playDONAIntro(AudioSource source)
    {
        playAudio(IntroAudio[2], source, 2.0f);
    }

    private IEnumerator stopNon_LoopAudio(AudioSource source)
    {
        yield return new WaitForSeconds(source.audio.clip.length);
        source.loop = false;
        source.volume = 0.0f;
        source.Stop();
    }

    private IEnumerator playVOIntro(AudioSource source)
    {
        playAudio(IntroAudio[0], source, 2.0f);
        yield return new WaitForSeconds(source.audio.clip.length);
        playAudio(IntroAudio[1], source, 2.0f);
    }

    private IEnumerator playVoDialogue(AudioSource source)
    {
        if (VO_DIndex == VODialogue.Count-1)
        {
            VO_DIndex = 0;
        }
        playAudio(VODialogue[VO_DIndex++], source, 2.0f);
        yield return new WaitForSeconds(source.audio.clip.length);
        playAudio(VODialogue[VODialogue.Count - 1], source, 2.0f);
    }
}
