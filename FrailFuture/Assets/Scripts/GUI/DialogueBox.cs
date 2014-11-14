using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueBox : MonoBehaviour {

    public List<Text> textBoxs;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EnableMyText(int indexOfTestToUse)
    {
        DisableMyText();
        if (indexOfTestToUse < textBoxs.Count && indexOfTestToUse >= 0)
        {
            textBoxs[indexOfTestToUse].gameObject.SetActive(true);
        }
    }

    public void DisableMyText()
    {
        foreach(Text tb in textBoxs)
        {
            tb.gameObject.SetActive(false);
        }
    }
}
