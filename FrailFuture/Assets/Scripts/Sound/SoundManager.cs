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
    public List<AudioClip> EndingsFromBestToWorst;

    public List<AudioClip> IntroAudio;

    public AudioClip OutroAllBeginAudio;
    public List<AudioClip> OutroNeutralityAudio;
    public List<AudioClip> OutroEndingAudio;
    public float TotalOutroAudioTime {get; private set;}

    public List<AudioClip> DONADialoguePos;
    public List<AudioClip> DONADialogueNeg;

    public List<AudioClip> RoomDialogue;
    public List<AudioClip> VODialogue;
    public int NumberOfVoDialogue {get {return numOfVoDialogue;}}

    public static SoundManager soundManager;
    private int numOfVoDialogue;
    private int VO_DIndex = 0;
    private List<int> DONAPosResponsesUsed;
    private List<int> DONANegResponsesUsed;
    private List<int> DONAPosDialougueUsed;
    private List<int> DONANegDialougueUsed;
    private int maxAllDONAResetPointAt = 5;
    private System.Random randMaker;

    private const float normalDonaVol = 2.5f;
    private const float endSceneDonaVol = 3.0f;
    private const float movementVol = 1.0f;
    private const float VoVol = 2.0f;

    void Awake()
    {
        if (soundManager == null)
        {
            DontDestroyOnLoad(gameObject);
            soundManager = this;
            randMaker = new System.Random(System.DateTime.Now.GetHashCode());
            DONAPosResponsesUsed = new List<int>();
            DONANegResponsesUsed = new List<int>();
            DONAPosDialougueUsed = new List<int>();
            DONANegDialougueUsed = new List<int>();
            numOfVoDialogue = VODialogue.Count - 1;
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

   public bool getAi_IsTalking()
    {
        return GameObject.Find("AiSpeaker").audio.isPlaying;
    }

    //Player Movement Audio
	public void playWalkSound()
    {
        playAudio(playerMovementSounds[0], GameObject.Find("Player").audio, movementVol, true);
    }
    public void stopPlayerSounds()
    {
        stopAudio(GameObject.Find("Player").audio);
    }
    public void playJumpSound()
    {
        playAudio(playerMovementSounds[1], GameObject.Find("Player").audio, movementVol, false);
    }
    public void playLandSound()
    {
        playAudio(playerMovementSounds[2], GameObject.Find("Player").audio, movementVol, false);
    }

    //getAi Outro
    public void playOutro()
    {
        int AiNeutrality = (int)GameControl.control.Ai.getNeutralityState();
        AudioClip begin = OutroAllBeginAudio;
        AudioClip middle = OutroNeutralityAudio[AiNeutrality];
        AudioClip end;
        if (GameControl.control.Ai.getNeutralityValue() > 0)
        {
            end = OutroEndingAudio[0];
        }
        else
        {
            end  = OutroEndingAudio[1];
        }
        TotalOutroAudioTime = begin.length + middle.length + end.length + 0.5f;
        StartCoroutine(playAllOutro(GameObject.Find("AiSpeaker").audio, begin, middle, end));
    }

    //getAi Dialogue
    public void playDONADialogue()
    {
        bool dialogueFound = false;
        int index = 0;
        //play random dialogue from DONA based on neutrality
        if (GameControl.control.Ai.getCurrentGraphIndex() == 0 && GameControl.control.Ai.getLastPickedInfo() == null)
        {
            playDONAIntro(GameObject.Find("AiSpeaker").audio);
        }
        else if (GameControl.control.Ai.getNeutralityValue() >= 0)
        {
            while (!dialogueFound)
            {
                 index = randMaker.Next(0, DONADialoguePos.Count);
                 dialogueFound = keepTrackOfAudioPlayed(DONAPosDialougueUsed, maxAllDONAResetPointAt, index);
            }
            playAudio(DONADialoguePos[index], GameObject.Find("AiSpeaker").audio, normalDonaVol);
        }
        else
        {
            while (!dialogueFound)
            {
                index = randMaker.Next(0, DONADialogueNeg.Count);
                dialogueFound = keepTrackOfAudioPlayed(DONANegDialougueUsed, maxAllDONAResetPointAt, index);
            }
            playAudio(DONADialogueNeg[index], GameObject.Find("AiSpeaker").audio, normalDonaVol);
        }
    }

    public void playVODialogue()
    {
        if (GameControl.control.Ai.getCurrentGraphIndex() == 0 && GameControl.control.Ai.getLastPickedInfo() == null)
        {
            playVOIntro(GameObject.Find("AiSpeaker").audio);
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
            playAudio(EndingsFromBestToWorst[(int)neutralityOfEnding], GameObject.Find("AiSpeaker").audio, normalDonaVol);
        }
    }

    //getAi Instructions
    public void playDirection(int doorToPick)
    {
        //doorToPick is one based
        if (doorToPick >= 1 && doorToPick <= GameControl.control.maxNumChoices)
        {
            int AiNeutrality = (int)GameControl.control.Ai.getNeutralityState();
            if (doorToPick == 1)
            {
                playAudio(AiDirectionsDoor1[AiNeutrality], GameObject.Find("AiSpeaker").audio, normalDonaVol);
            }
            else if (doorToPick == 2)
            {
                playAudio(AiDirectionsDoor2[AiNeutrality], GameObject.Find("AiSpeaker").audio, normalDonaVol);
            }

            else if (doorToPick == 3)
            {
                playAudio(AiDirectionsDoor3[AiNeutrality], GameObject.Find("AiSpeaker").audio, normalDonaVol);
            }
        }
    }

    public void playResponse()
    {
        PlayerData pd = GameControl.control.Ai.getLastPickedInfo();
        bool aiIsPos = GameControl.control.Ai.getNeutralityValue() >= 0;
        //grab random index based on having pos or neg neutrality
        bool goodIndex = false;
        int index = 0;
        while (!goodIndex)
        {
            index = randMaker.Next(2, (PickedRightDoor_Desired.Count));
            if (aiIsPos)
            {
                if (index % 2 == 0)
                {
                    index -= 1;
                }
                goodIndex = keepTrackOfAudioPlayed(DONAPosResponsesUsed, maxAllDONAResetPointAt, index);
            }
            else
            {
                if (index % 2 != 0)
                {
                    index = DONANegDialougueUsed.Contains(0) ? (index - 1) : 0;
                }
                goodIndex = keepTrackOfAudioPlayed(DONANegResponsesUsed, maxAllDONAResetPointAt, index);
            }
        }

        if (pd.picked == pd.delivered)
        {
            if (pd.picked == pd.desired)
            {
                playAudio(PickedRightDoor_Desired[index], GameObject.Find("AiSpeaker").audio, normalDonaVol);
            }
            else
            {
                playAudio(PickedRightDoor[index], GameObject.Find("AiSpeaker").audio, normalDonaVol);
            }
        }
        else
        {
            if (pd.picked == pd.desired)
            {
                playAudio(PickedWrongDoor_Desired[index], GameObject.Find("AiSpeaker").audio, normalDonaVol);
            }
            else
            {
                playAudio(PickedWrongDoor[index], GameObject.Find("AiSpeaker").audio, normalDonaVol);
            }
        }
    }

    private bool keepTrackOfAudioPlayed(List<int> container, int maxCount, int newIndex)
    {
        bool goodIndex = false;
        if (!container.Contains(newIndex))
        {
            container.Add(newIndex);
            goodIndex = true;

            if (container.Count == maxCount)
            {
                container.Clear();
                //make sure same indexes are not played in a row when reseting.
                container.Add(newIndex);
            }
        }
        return goodIndex;
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
        playAudio(IntroAudio[1], source, normalDonaVol);
    }
    private void playVOIntro(AudioSource source)
    {
        playAudio(IntroAudio[0], source, VoVol);
    }
    private IEnumerator playVoDialogue(AudioSource source)
    {
        if (VO_DIndex == VODialogue.Count-1)
        {
            VO_DIndex = 0;
        }
        playAudio(VODialogue[VO_DIndex++], source, VoVol);
        yield return new WaitForSeconds(source.audio.clip.length);
        playAudio(VODialogue[VODialogue.Count - 1], source, VoVol);
    }

    private IEnumerator playAllOutro(AudioSource source, AudioClip begin, AudioClip middle, AudioClip end)
    {
        playAudio(begin, source, endSceneDonaVol);
        yield return new WaitForSeconds(source.audio.clip.length);
        StartCoroutine(playMiddleAndEndAudio(source, middle,end));
    }

    private IEnumerator playMiddleAndEndAudio(AudioSource source, AudioClip middle, AudioClip end)
    {
        playAudio(middle, GameObject.Find("AiSpeaker").audio, endSceneDonaVol);
        yield return new WaitForSeconds(source.audio.clip.length);
        playAudio(end, GameObject.Find("AiSpeaker").audio, endSceneDonaVol);
    }
}