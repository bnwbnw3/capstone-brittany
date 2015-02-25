using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuickPlay_MenuManager : MonoBehaviour 
{
    public Menu CurrentMenu;
    public GameObject mainScene;
    public GameObject loadingScene;
    public GameObject loadingObj;

    private bool gameSettingsNotInited;
    void Start()
    {
        Screen.lockCursor = false;
        ShowMenu(CurrentMenu);
        mainScene.SetActive(true);
        loadingScene.SetActive(false);
        loadingObj.SetActive(false);
        gameSettingsNotInited = true;
    }

    void Update()
    {
        updateGameValues();
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

    public void StartGame()
    {
        GameControl.control.SetUpQuickGame();
        StartCoroutine(waitToLoadGame(2.0f));
    }

    IEnumerator waitToLoadGame(float waitTime)
    {
        mainScene.SetActive(false);
        loadingScene.SetActive(true);
        loadingObj.SetActive(true);
        CurrentMenu.IsOpen = false;
        Screen.lockCursor = true;
        yield return new WaitForSeconds(waitTime);
        ScreenFader.screenFader.makeSolid("GameScene", 2.0f);
    }

    public void updateGameValues()
    {
        if(gameSettingsNotInited)
        {
            GameControl.control.SetToDefaultGameValues();
            gameSettingsNotInited = false;
        }
    }
}