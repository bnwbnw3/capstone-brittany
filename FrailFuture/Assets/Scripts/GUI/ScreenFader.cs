using UnityEngine;
using System.Collections;
public class ScreenFader : MonoBehaviour
{
    public bool setToClearEditor = true;      // Whether or not the scene is still fading in.
    public CanvasGroup groupToFade;
    public float clearValue = 0;
    public float solidValue = 1;

    private bool setToSolid;
    private bool setToClear;
    private string loadLevelName;
    private float fadeSpeed;

    public static ScreenFader screenFader;
    void Awake()
    {
        if (screenFader == null)
        {
            DontDestroyOnLoad(gameObject);
            setToSolid = false;
            fadeSpeed = 1.5f;
            loadLevelName = "";
            setToClear = setToClearEditor;
            screenFader = this;
            if (setToClear == true)
            {
                groupToFade.alpha = 1;
            }
            else
            {
                groupToFade.alpha = 0;
            }
        }
        else if (screenFader != this)
        {
            Destroy(gameObject);
        }

    }

    void Update()
    {
        if (setToClear)
        {
            SetToClear();
        }
        else if (setToSolid)
        {
            SetToSolid(loadLevelName, fadeSpeed);
        }
    }

    public void makeSolid(string loadLevelName = "", float fadingSpeed = 1.5f)
    {
        setToSolid = true;
        setToClear = false;
        this.gameObject.SetActive(true);
        this.fadeSpeed = fadingSpeed;
        this.loadLevelName = loadLevelName;
    }

    void SetToClear(float fadeSpeed = 1.0f)
    {
        // Fade the texture to clear.
        FadeToClear(fadeSpeed);

        // If the texture is almost clear...
        if (groupToFade.alpha <= 0.05f)
        {
            groupToFade.alpha = clearValue;
            //make canvas go away
            this.gameObject.SetActive(false);
            setToClear = false;
        }
    }

    private void SetToSolid(string levelToLoad, float fadeSpeed)
    {
        if(!setToClear && setToSolid)
        {
            // Make sure the canvas is enabled.
            this.gameObject.SetActive(true);

            FadeToBlack(fadeSpeed);

            // If the screen is almost black...
            if (groupToFade.alpha >= 0.95f)
            {
                groupToFade.alpha = solidValue;
                setToSolid = false;
                setToClear = true;
                if(levelToLoad.CompareTo("") != 0)
                {
                    Application.LoadLevel(levelToLoad);
                }
            }
        }
    }

    void FadeToClear(float fadeSpeed)
    {
        groupToFade.alpha = Mathf.Lerp(groupToFade.alpha, clearValue, fadeSpeed * Time.deltaTime);
    }

    void FadeToBlack(float fadeSpeed)
    {
        groupToFade.alpha = Mathf.Lerp(groupToFade.alpha, solidValue, fadeSpeed * Time.deltaTime);
    }

}