using UnityEngine;
using System.Collections;

public class ThemeMusicPlayer : MonoBehaviour {

    public float multiplier = 0.3f;
    public static ThemeMusicPlayer self;

    void Awake()
    {
        //Edit script Awake call through Edit->ProjectSettings->Script Order
        if (self == null)
        {
            DontDestroyOnLoad(gameObject);
            self = this;
        }
        else if (self != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audio.volume = GameControl.control.BackgroundMusicVolume * multiplier;
    }
    void Update()
    {
        audio.volume = GameControl.control.BackgroundMusicVolume * multiplier;
    }
}