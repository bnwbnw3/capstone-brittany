using UnityEngine;
using System.Collections;

public class SimpleMainMenu : MonoBehaviour
{
    public Menu CurrentMenu;
    public AudioSource narration;
    private bool turnNarationOff;
    // Use this for initialization
    void Start()
    {
        Screen.lockCursor = false;
        turnNarationOff = false;
        ShowMenu(CurrentMenu);
    }

    void Update()
    {
        if (turnNarationOff)
        {
            if(narration.volume > 0)
            {
                float newVol = 0.5f * Time.deltaTime;
                narration.volume  -= newVol;
            }
            else
            {
                narration.Stop();
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
        CurrentMenu.IsOpen = true;
    }

    public void QuitMenus()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        turnNarationOff = true;
        ScreenFader.screenFader.makeSolid("MainMenu", 2.0f);
    }
}