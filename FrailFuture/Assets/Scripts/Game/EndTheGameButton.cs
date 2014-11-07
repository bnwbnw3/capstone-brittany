using UnityEngine;
using System.Collections;

public class EndTheGameButton : MonoBehaviour {

    public void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            transform.collider.isTrigger = false;
            //make button disapear from view
            GameObject resetButton = GameObject.Find("ResetButton");
            Vector3 oldPos = resetButton.transform.position;
            resetButton.transform.position = new Vector3(oldPos.x, oldPos.y - 10, oldPos.z);

            //Fade out

            Screen.showCursor = true;
            ScreenFader.screenFader.makeSolid("MainMenu", 2.0f);
        }
    }
}
