using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public GameObject player;
    public GameObject HUDCameraOBJ;
    public Menu ResetMenuTo;
    public Menu CurrentMenu;
    private bool HUDOpen;

    public void Start()
    {
        HUDOpen = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !SoundManager.soundManager.getIsAiTalking())
        {
            HUDOpen = !HUDOpen;

            if (HUDOpen)
            {
                Screen.lockCursor = false;
                player.SetActive(false);
                HUDCameraOBJ.SetActive(true);
                ShowMenu(CurrentMenu);
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void ShowMenu(Menu menu)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.IsOpen = false;

            ChangeSettings options = GameObject.Find("OptionsMenuContainer").GetComponentInChildren<ChangeSettings>();
            options.invertX.isOn = GameControl.control.InvertX;
            options.invertY.isOn = GameControl.control.InvertY;
            options.mouseSensitivity.value = GameControl.control.MouseSensitivity;
            options.backgroundMusic.value = GameControl.control.BackgroundMusicVolume;
            options.soundEffects.value = GameControl.control.SoundEffectsVolume;
        }

        CurrentMenu = menu;

        if (HUDOpen)
        {
            CurrentMenu.IsOpen = true;
        }
    }

    public void SaveGame(InputField Field)
    {
        Screen.lockCursor = false;
        string fileName = Field.text.text;
        GameControl.control.Save(fileName);
        GameControl.control.LastKnownFileName = fileName;
    }

    public void ResumeGame()
    {
        Screen.lockCursor = true;
        player.SetActive(true);
        HUDCameraOBJ.SetActive(false);
        CurrentMenu.IsOpen = false;
        HUDOpen = false;
        CurrentMenu = ResetMenuTo;
    }

    public void LoadMainMenu()
    {
        Screen.lockCursor = false;
        GameControl.control.Save(GameControl.tempAutoSaveFileLocation);
        NodeManager.nodeManager.resetAllHallways();
        ScreenFader.screenFader.makeSolid("MainMenu", 2.0f);
    }

    public void QuitGame()
    {
        Screen.lockCursor = false;
        GameControl.control.Save(GameControl.tempAutoSaveFileLocation);
        Application.Quit();
    }
}