using UnityEngine;
using System.Collections;

public class ReturnToMainMenu : MonoBehaviour {

    public RailPointManager RPM;
    public float secondsToDelayLoadMainMenu = 1.0f;
    private bool currentlyLoading;

    void Start()
    {
        Screen.lockCursor = true;
        currentlyLoading = false;
    }

	void Update () 
    {
        if (RPM.AtEndRailPoint && !currentlyLoading)
        {
            StartCoroutine(WaitToLoad());
        }
	}

    private IEnumerator WaitToLoad()
    {
        currentlyLoading = true;
        yield return new WaitForSeconds(secondsToDelayLoadMainMenu);
        ScreenFader.screenFader.makeSolid("MainMenu", 2.0f);

    }
}
