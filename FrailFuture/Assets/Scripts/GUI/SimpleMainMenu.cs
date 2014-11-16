using UnityEngine;
using System.Collections;

public class SimpleMainMenu : MonoBehaviour
{
    public Menu CurrentMenu;
    // Use this for initialization
    void Start()
    {
        Screen.lockCursor = false;
        ShowMenu(CurrentMenu);
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
        ScreenFader.screenFader.makeSolid("MainMenu", 2.0f);
    }
}
