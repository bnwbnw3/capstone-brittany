using UnityEngine;
using System.Collections;

public class TriggerEnableText : MonoBehaviour {

    public DialogueBox holderOfText;
    private System.Random rand;
	// Use this for initialization
	void Start () {
        rand = new System.Random();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider c)
    {
        int index = rand.Next(0, holderOfText.textBoxs.Count);
        holderOfText.EnableMyText(index);
    }
}
