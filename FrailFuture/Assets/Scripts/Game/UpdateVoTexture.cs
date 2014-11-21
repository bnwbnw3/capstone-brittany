using UnityEngine;
using System.Collections;

public class UpdateVoTexture : MonoBehaviour {

    public Material OnTexture, OffTexuture;

	void Update () 
    {
       this.renderer.material = SoundManager.soundManager.getAi_IsTalking()? OnTexture:OffTexuture;
	}
}