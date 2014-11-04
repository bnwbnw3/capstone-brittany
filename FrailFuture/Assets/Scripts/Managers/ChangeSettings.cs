using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeSettings : MonoBehaviour 
{
   public Toggle invertX;
   public Toggle invertY;
   public Slider mouseSensitivity;
   public Slider backgroundMusic;
   public Slider soundEffects;
   public MenuManager menuManager;
   public HUDManager HUDManager;

   public void Awake()
   {
       invertX.isOn = GameControl.control.InvertX;
       invertY.isOn = GameControl.control.InvertY;
       mouseSensitivity.value = GameControl.control.MouseSensitivity;
       backgroundMusic.value = GameControl.control.BackgroundMusicVolume;
       soundEffects.value = GameControl.control.SoundEffectsVolume;
   }

    public void updateSettings(Menu toLoadAfter)
    {
        setInvertX(invertX.isOn);
        setInvertY(invertY.isOn);
        setSensitivity(mouseSensitivity.value);
        setBackgroundMusicVol(backgroundMusic.value);
        setSoundEffectsVol(soundEffects.value);
        if (menuManager != null)
        {
            menuManager.ShowMenu(toLoadAfter);
        }
        else if (HUDManager != null)
        {
            HUDManager.ShowMenu(toLoadAfter);
        }
    }

    private void setInvertY(bool value)
    {
        GameControl.control.InvertY = value;
    }
    private void setInvertX(bool value)
    {
        GameControl.control.InvertX = value;
    }
    private void setSensitivity(float value)
    {
        GameControl.control.MouseSensitivity = value;
    }
    private void setBackgroundMusicVol(float value)
    {
        GameControl.control.BackgroundMusicVolume = value;
    }
    private void setSoundEffectsVol(float value)
    {
        GameControl.control.SoundEffectsVolume = value;
    }
}