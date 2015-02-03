using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour
{
	public GUISkin skin;
    public Texture winScreenTexture;
	
	void Update () 
	{
		if(Input.anyKey)
			Application.LoadLevel("main_menu");
	}

    void OnGUI()
    {
	    GUI.skin = skin;
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),winScreenTexture);
    }
}
