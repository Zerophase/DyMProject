using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour
{
	public GUISkin skin;
    public Texture winScreenTexture;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.anyKey)
			Application.LoadLevel("main_menu");
	}

    void OnGUI()
    {
	    GUI.skin = skin;
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),winScreenTexture);

	    GUI.Label(new Rect(Screen.width/2, Screen.height/2, 100, 50), "Press any key to Return to Menu");
    }
}
