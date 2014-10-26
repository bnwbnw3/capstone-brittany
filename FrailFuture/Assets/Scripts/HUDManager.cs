using UnityEngine;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public GameObject player;
    public Menu CurrentMenu;
    private bool HUDOpen;


    public void Start()
    {
        //ShowMenu(CurrentMenu);
        HUDOpen = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HUDOpen = !HUDOpen;

            if (HUDOpen)
            {
                player.SetActive(false);
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
        }

        CurrentMenu = menu;

        if (HUDOpen)
        {
            CurrentMenu.IsOpen = true;
        }
    }


    public void ResumeGame()
    {
        player.SetActive(true);
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