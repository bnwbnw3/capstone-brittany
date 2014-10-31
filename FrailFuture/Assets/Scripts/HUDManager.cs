﻿using UnityEngine;
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
        //breaks if ai is talking while half pausing game. Tried to debug and couldn't get to source.
        if (Input.GetKeyDown(KeyCode.Escape) && !SoundManager.soundManager.getIsAiTalking())
        {
            HUDOpen = !HUDOpen;

            if (HUDOpen)
            {
                player.SetActive(false);
                HUDCameraOBJ.SetActive(true);
                ShowMenu(CurrentMenu);
            }
            else
            {
                ResumeGame();
                CurrentMenu = ResetMenuTo;
            }
        }
    }

    public void ShowMenu(Menu menu)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.IsOpen = false;

            ChangeSettings options = GameObject.Find("OptionsMenuContainer").GetComponentInChildren<ChangeSettings>();
            options.invertX.isOn = GameControl.control.invertX;
            options.invertY.isOn = GameControl.control.invertY;
            options.mouseSensitivity.value = GameControl.control.mouseSensitivity;
            options.backgroundMusic.value = GameControl.control.backgroundMusicVolume;
            options.soundEffects.value = GameControl.control.soundEffectsVolume;
        }

        CurrentMenu = menu;

        if (HUDOpen)
        {
            CurrentMenu.IsOpen = true;
        }
    }

    public void SaveGame(InputField Field)
    {
        string fileName = Field.text.text + ".dat";
        GameControl.control.Save(fileName);
    }

    public void ResumeGame()
    {
        player.SetActive(true);
        HUDCameraOBJ.SetActive(false);
        CurrentMenu.IsOpen = false;
        HUDOpen = false;
    }

    public void LoadMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}