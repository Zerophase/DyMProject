using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public Texture2D logo;

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
    }

    void MainMenuScreen()
    {
        GUI.skin = skin;

        GUI.BeginGroup(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 350, 615, 500));

        //logo
        GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);

        //buttons
        if (GUI.Button(new Rect(225, 210, 150, 50), "start"))
        {

        }

        if (GUI.Button(new Rect(225, 270, 150, 50), "options"))
        {
           GUIMenu = "Options";
        }

        if (GUI.Button(new Rect(225, 330, 150, 50), "credits"))
        {
            GUIMenu = "Credits";
        }

        if (GUI.Button(new Rect(225, 390, 150, 50), "exit"))
        {
            Application.Quit();
        }

         GUI.EndGroup();
    }

    void OptionsScreen()
    {
        GUI.skin = skin;

        GUI.BeginGroup(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 350, 615, 500));

        //logo
        GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);

        //buttons
        if (GUI.Button(new Rect(225, 390, 150, 50), "back"))
        {
            GUIMenu = "MainMenu";
        }

        //sliders        
        musicValue = GUI.HorizontalSlider(new Rect(210, 235, 200, 20), musicValue, 0.0F, 1.00F);
        PlayerPrefs.SetFloat("MusicVolume", musicValue);

        GUI.Label(new Rect(280, 210, 254, 47), "Music");


        soundValue = GUI.HorizontalSlider(new Rect(210, 300, 200, 20), soundValue, 0.0F, 1.00F);
        PlayerPrefs.SetFloat("SoundVolume", soundValue);

        GUI.Label(new Rect(280, 275, 254, 47), "Sound");

        GUI.EndGroup();
    }

    void Credits()
    {
        GUI.skin = skin;

        GUI.BeginGroup(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 350, 615, 500));

        //logo
        GUI.Label(new Rect(0, 0, logo.width, logo.height), logo);

        GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 + -20, logo.width, logo.height), logo);

        //buttons
        if (GUI.Button(new Rect(225, 390, 150, 50), "back"))
        {
            GUIMenu = "MainMenu";
        }

        GUI.EndGroup();

    }


}
