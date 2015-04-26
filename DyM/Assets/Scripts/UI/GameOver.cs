using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	public GUIStyle gameOver;
	public GUIStyle thankYou;
	public GUIStyle credits;
	public GUIStyle pressButton;

	public Texture2D logo;

	void Update () 
	{
		if(Input.anyKey)
			Application.LoadLevel("main_menu");
	}

	void OnGUI()
	{
		pressButton.fontSize = (int)(25.0f * (float)(Screen.width)/1920.0f);
		credits.fontSize = (int)(25.0f * (float)(Screen.width)/1920.0f);
		gameOver.fontSize = (int)(100.0f * (float)(Screen.width)/1920.0f);
		thankYou.fontSize = (int)(50.0f * (float)(Screen.width)/1920.0f);

		GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

		GUI.Label(new Rect(Screen.width / 3, Screen.height / 15, Screen.width / 3, Screen.height / 3), logo);

		GUI.Label(new Rect(Screen.width / 3.3f, Screen.height / 2.8f, Screen.width / 2.5f, Screen.height / 10), "GAmE OVER", gameOver);
		GUI.Label(new Rect(Screen.width / 3.2f, Screen.height / 2.3f, Screen.width / 2.5f, Screen.height / 10), "thAnks For plAying", thankYou);
		GUI.Label(new Rect(Screen.width / 2.7f, Screen.height / 2, Screen.width / 2.5f, Screen.height / 10), "Press Any Key to continue", pressButton);

		GUI.Label(new Rect(Screen.width / 2.705f, Screen.height / 1.6f, Screen.width / 4, Screen.height / 25), " Alex Lueck - LeAd Design", credits);
		GUI.Label(new Rect(Screen.width / 3.133f, Screen.height / 1.5f, Screen.width / 2.8f, Screen.height / 25), " MichAel Lojkovic - LeAd ProgrAmmer", credits);
		GUI.Label(new Rect(Screen.width / 2.808f, Screen.height / 1.41f, Screen.width / 3.7f, Screen.height / 25), " Justin Terry - ProgrAmmer", credits);
		GUI.Label(new Rect(Screen.width / 2.88f, Screen.height / 1.33f, Screen.width / 4.5f, Screen.height / 25), " PAtric HArmon - Artist", credits);
		GUI.Label(new Rect(Screen.width / 3.238f, Screen.height / 1.258f, Screen.width / 3, Screen.height / 25), " Geoff Lightbourn - Sound LiAson", credits);
		GUI.Label(new Rect(Screen.width / 2.81f, Screen.height / 1.19f, Screen.width / 4.2f, Screen.height / 25), " KAnoA Doblin - Composer", credits);
		GUI.Label(new Rect(Screen.width / 3.2f, Screen.height / 1.135f, Screen.width / 3.2f, Screen.height / 25), " JessicA BorlovAn - UI Scripting", credits);

		GUI.EndGroup();
	}
}
