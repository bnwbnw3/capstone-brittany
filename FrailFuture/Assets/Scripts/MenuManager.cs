using UnityEngine;
using UnityEngine.UI;
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

    public void LoadGame(InputField Field)
    {
        string fileName = Field.text.text + ".dat";
        GameControl.control.Load(fileName);
        if (GameControl.ableToLoadGame)
        {
            GameControl.ableToLoadGame = false;
            StartCoroutine(waitToLoadGame(5.0f));
        }
    }

    public void StartGame()
    {
        GameControl.control.makeBeginnerAi();
        StartCoroutine(waitToLoadGame(5.0f));
    }

    IEnumerator waitToLoadGame(float waitTime)
    {
        mainScene.SetActive(false);
        loadingScene.SetActive(true);
        loadingObj.SetActive(true);
        CurrentMenu.IsOpen = false;
        yield return new WaitForSeconds(waitTime);
        Application.LoadLevel("GameScene");
    }
}
