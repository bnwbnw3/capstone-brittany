using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class SoundManager : MonoBehaviour
{
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

    //Do pattern stating later.
    //public Dictionary<string, List<AudioClip>> patterns;

    public static SoundManager soundManager;
    private int VO_DIndex = 0;
    private List<int> DONAResponsesUsed;
    private List<int> DONADialougueUsed;
    private System.Random randMaker;

    void Awake()
    {
        if (soundManager == null)
        {
            DontDestroyOnLoad(gameObject);
            soundManager = this;
            randMaker = new System.Random(System.DateTime.Now.GetHashCode());
            DONAResponsesUsed = new List<int>();
            DONADialougueUsed = new List<int>();
            if (GameControl.control.WasLoaded)
            {
                VO_DIndex = randMaker.Next(0, VODialogue.Count);
            }
        }
        else if (soundManager != this)
        {
            Destroy(gameObject);
        }
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

    //getAi Dialogue
    public void playDONADialogue()
    {
        bool dialogueFound = false;
        int index = 0;
        //play random dialogue from DONA based on neutrality
        if (GameControl.control.getAi.getCurrentGraphIndex() == 0 && GameControl.control.getAi.getLastPickedInfo() == null)
        {
            playDONAIntro(GameObject.Find("AiSpeaker").audio);
        }
        else if (GameControl.control.getAi.getNeutralityValue() >= 0)
        {
            while (!dialogueFound)
            {
                if (DONADialougueUsed.Count == DONADialoguePos.Count)
                {
                    DONADialougueUsed.Clear();
                }
                 index = randMaker.Next(0, DONADialoguePos.Count);
                 if (!DONADialougueUsed.Contains(index))
                 {
                     DONADialougueUsed.Add(index);
                     dialogueFound = true;
                 }
            }
            playAudio(DONADialoguePos[index], GameObject.Find("AiSpeaker").audio, 2.0f);
        }
        else
        {
            while (!dialogueFound)
            {
                if (DONADialougueUsed.Count == DONADialogueNeg.Count)
                {
                    DONADialougueUsed.Clear();
                }
                index = randMaker.Next(0, DONADialogueNeg.Count);
                if (!DONADialougueUsed.Contains(index))
                {
                    DONADialougueUsed.Add(index);
                    dialogueFound = true;
                }
            }
            playAudio(DONADialogueNeg[index], GameObject.Find("AiSpeaker").audio, 2.0f);
        }
    }

    public void playVODialogue()
    {
        if (GameControl.control.getAi.getCurrentGraphIndex() == 0 && GameControl.control.getAi.getLastPickedInfo() == null)
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
        if (neutralityOfEnding != NeutralityTypes.None)
        {
            playAudio(EngingsFromBestToWorst[(int)neutralityOfEnding], GameObject.Find("AiSpeaker").audio, 2.0f);
        }
    }

    //getAi Instructions
    public void playDirection(int doorToPick)
    {
        //doorToPick is one based
        if (doorToPick >= 1 && doorToPick <= GameControl.control.maxNumChoices)
        {
            int AiNeutrality = (int)GameControl.control.getAi.getNeutralityState();
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
    }

    public void playResponse()
    {
        PlayerData pd = GameControl.control.getAi.getLastPickedInfo();
        bool aiIsPos = GameControl.control.getAi.getNeutralityValue() >= 0;
        //grab random index based on having pos or neg neutrality
        bool foundIndex = false;
        int index = 0;
        if (DONAResponsesUsed.Count == PickedRightDoor.Count)
        {
            DONAResponsesUsed.Clear();
        }
        while (!foundIndex)
        {
            index = randMaker.Next(2, (PickedRightDoor_Desired.Count));
            if (index % 2 == 0 && aiIsPos)
            {
                index -= 1;
            }
            if (index % 2 != 0 && !aiIsPos)
            {
                index -= 1;
            }
            if (!DONAResponsesUsed.Contains(index))
            {
                DONAResponsesUsed.Add(index);
                foundIndex = true;
            }
        }
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
        source.volume = volume*GameControl.control.SoundEffectsVolume;
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

    private void playDONAIntro(AudioSource source)
    {
        playAudio(IntroAudio[2], source, 2.0f);
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