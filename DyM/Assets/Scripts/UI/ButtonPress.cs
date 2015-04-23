using UnityEngine;
using System.Collections;
using Assets.Scripts.CustomInputManager;

public class ButtonPress : MonoBehaviour {
	

	// Update is called once per frame
	void Update () 
	{
		if(InputManager.Jumping() || Input.anyKeyDown)
		{
			AutoFade.LoadLevel(3, 3, 1, Color.black);
		}

	}
}
