using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GrabInputFieldTextFromGC : MonoBehaviour {

	// Use this for initialization
    void Awake()
    {
        InputField field = this.gameObject.GetComponent<InputField>();
        field.value = GameControl.control.LastKnownFileName;
        field.text.text = GameControl.control.LastKnownFileName;
    }
}
