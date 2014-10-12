using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
    public List<AudioClip> sounds;

    public static SoundManager soundManager;
    void Awake()
    {
        if (soundManager == null)
        {
            DontDestroyOnLoad(gameObject);
            soundManager = this;
        }
        else if (soundManager != this)
        {
            Destroy(gameObject);
        }
    }
	public void playWalkSound(AudioSource source)
    {
       source.clip = sounds[0];
       source.loop = true;
       source.volume = 0.5f;
       source.Play();
    }
    public void stopWalkSound(AudioSource source)
    {
       source.loop = false;
       source.Stop();
    }
}
