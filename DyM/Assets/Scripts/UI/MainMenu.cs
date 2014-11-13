using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public Texture2D logo;

	public Texture2D controls;

    public GUISkin skin;

    public string GUIMenu = "";

    public float musicValue;
    public float soundValue;

    public GameObject musicManager;
    public GameObject soundManager;

    void Start()
    {
        GUIMenu = "MainMenu";

        PlayerPrefs.SetFloat("MusicVolume", 1.00f);
        musicValue = PlayerPrefs.GetFloat("MusicVolume");

        PlayerPrefs.SetFloat("SoundVolume", 1.00f);
        soundValue = PlayerPrefs.GetFloat("SoundVolume");
    }


    void OnGUI()
    {
        if (GUIMenu == "MainMenu")
            MainMenuScreen();

        if (GUIMenu == "Options")
            OptionsScreen();

        if (GUIMenu == "Credits")
            Credits();
		
		if (GUIMenu == "Controls")
			Controls();
    }

    void MainMenuScreen()
    {
        GUI.skin = skin;

        GUI.BeginGroup(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 350, 615, 550));

        //logo
        GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);

        //buttons
        if (GUI.Button(new Rect(225, 210, 150, 50), "start"))
        {
			Application.LoadLevel("Level_01");
        }

        if (GUI.Button(new Rect(225, 270, 150, 50), "options"))
        {
           GUIMenu = "Options";
        }

        if (GUI.Button(new Rect(225, 330, 150, 50), "controls"))
        {
            GUIMenu = "Controls";
        }

		if (GUI.Button(new Rect(225, 390, 150, 50), "credits"))
		{
			GUIMenu = "Credits";
		}

        if (GUI.Button(new Rect(225, 450, 150, 50), "exit"))
        {
            Application.Quit();
        }

         GUI.EndGroup();
    }

    void OptionsScreen()
    {
        GUI.skin = skin;

		GUI.BeginGroup(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 350, 615, 550));

        //logo
        GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);

        //buttons
		if (GUI.Button(new Rect(225, 450, 150, 50), "back"))
		{
			GUIMenu = "MainMenu";
		}

        //sliders        
        musicValue = GUI.HorizontalSlider(new Rect(210, 235, 200, 20), musicValue, 0.0F, 1.00F);
        PlayerPrefs.SetFloat("MusicVolume", musicValue);

        GUI.Label(new Rect(185, 210, 254, 47), "Music");


        soundValue = GUI.HorizontalSlider(new Rect(210, 300, 200, 20), soundValue, 0.0F, 1.00F);
        PlayerPrefs.SetFloat("SoundVolume", soundValue);

        GUI.Label(new Rect(185, 275, 254, 47), "Sound");

        GUI.EndGroup();
    }

    void Credits()
    {
        GUI.skin = skin;

		GUI.BeginGroup(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 350, 615, 550));

        //logo
        GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);

		//credits
		GUI.Label(new Rect(125, 225, 350, 150), " Alex Lueck - Lead Design");
		GUI.Label(new Rect(53, 243, 500, 150), " Michael Lojkovic - Lead Programmer");
		GUI.Label(new Rect(42, 261, 500, 150), " Justin Terry - Programmer");
		GUI.Label(new Rect(0, 279, 500, 150), " Patric Harmon - Artist");
		GUI.Label(new Rect(18, 297, 500, 150), " Geoff Lightbourn - Sound Liason");
		GUI.Label(new Rect(28, 315, 500, 150), " Kanoa Doblin - Composer");
		GUI.Label(new Rect(8, 333, 500, 150), " Jessica Borlovan - UI Scripting");


        //buttons
		if (GUI.Button(new Rect(225, 450, 150, 50), "back"))
		{
			GUIMenu = "MainMenu";
		}

        GUI.EndGroup();

    }

	void Controls()
	{
		GUI.skin = skin;
		
		GUI.BeginGroup(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 350, 615, 550));
		
		//logo
		GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);

		GUI.Label(new Rect(0, 215, controls.width, controls.height), controls);
		GUI.Label(new Rect(350, 275, 250, 200), "To Plane Shift and Stay on the plane hold the Plane Shift Button down. To Dodge tap the plane Shift Button.");
		
		//buttons
		if (GUI.Button(new Rect(225, 450, 150, 50), "back"))
		{
			GUIMenu = "MainMenu";
		}
		
		GUI.EndGroup();
		
	}


}
