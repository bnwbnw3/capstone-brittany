using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeSettings : MonoBehaviour {

   public Toggle invertX;
   public Toggle invertY;
   public Slider mouseSensitivity;
   public Slider backgroundMusic;
   public Slider soundEffects;
   public MenuManager menuManager;
   public HUDManager HUDManager;

   public void Awake()
   {
       invertX.isOn = GameControl.control.invertX;
       invertY.isOn = GameControl.control.invertY;
       mouseSensitivity.value = GameControl.control.mouseSensitivity;
       backgroundMusic.value = GameControl.control.backgroundMusicVolume;
       soundEffects.value = GameControl.control.soundEffectsVolume;
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
        GameControl.control.invertY = value;
    }

    private void setInvertX(bool value)
    {
        GameControl.control.invertX = value;
    }

    private void setSensitivity(float value)
    {
        GameControl.control.mouseSensitivity = value;
    }
    private void setBackgroundMusicVol(float value)
    {
        GameControl.control.backgroundMusicVolume = value;
    }

    private void setSoundEffectsVol(float value)
    {
        GameControl.control.soundEffectsVolume = value;
    }
}
