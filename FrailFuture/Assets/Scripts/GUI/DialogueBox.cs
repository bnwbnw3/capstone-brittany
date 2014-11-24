using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueBox : MonoBehaviour {

    public List<Text> textBoxs;
    public int IndexInUse { private set; get; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EnableMyText(int indexOfTestToUse)
    {
        IndexInUse = indexOfTestToUse;
        DisableMyText();
        if (IndexInUse < textBoxs.Count && IndexInUse >= 0)
        {
            textBoxs[IndexInUse].gameObject.SetActive(true);
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
