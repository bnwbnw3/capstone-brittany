using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour 
{
    public Menu CurrentMenu;
    public GameObject mainScene;
    public GameObject loadingScene;
    public GameObject loadingObj;


    public void Start()
    {
        ShowMenu(CurrentMenu);
        mainScene.SetActive(true);
        loadingScene.SetActive(false);
        loadingObj.SetActive(false);
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
        mainScene.SetActive(false);
        loadingScene.SetActive(true);
        loadingObj.SetActive(true);
        CurrentMenu.IsOpen = false;
        StartCoroutine(waitToLoadGame(5.0f));
    }

    IEnumerator waitToLoadGame(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Application.LoadLevel("GameScene");
    }
}
