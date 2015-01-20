using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour 
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

            ChangeSettings options = GameObject.Find("OptionsMenuContainer").GetComponentInChildren<ChangeSettings>();
            options.invertX.isOn = GameControl.control.InvertX;
            options.invertY.isOn = GameControl.control.InvertY;
            options.mouseSensitivity.value = GameControl.control.MouseSensitivity;
            options.backgroundMusic.value = GameControl.control.BackgroundMusicVolume;
            options.soundEffects.value = GameControl.control.SoundEffectsVolume;
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
        string fileName = Field.text;
        GameControl.control.Load(fileName);
        if (GameControl.control.AbleToLoadGame)
        {
            GameControl.control.LastKnownFileName = fileName;
            GameControl.control.AbleToLoadGame = false;
            StartCoroutine(waitToLoadGame(4.0f));
        }
        else
        {
            Field.text = GameControl.control.LastKnownFileName;
        }
    }

    public void StartGame()
    {
        StartCoroutine(waitToLoadGame(4.0f));
    }

    public void playIntroduction()
    {
        ScreenFader.screenFader.makeSolid("destroyed_city", 2.0f);
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