using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour 
{
    public Slider Slider;

    //To be used from a button click:
    /// <summary>
    /// Must be public
    /// return void
    /// have 0 to 1 params
    /// </summary>
    public void DoSomething()
    {
        Debug.Log("Slider value: "+ Slider.value);
    }

    public void DoSomethingWithASlider(Slider slider)
    {
        Debug.Log(slider.value.ToString());
    }
	
}
