using System;
using System.Collections.Generic;
using Assets.Scripts.Utilities;
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
	private enum Menus
	{
		MAIN_MENU,
		OPTIONS,
		CREDITS,
		CONTROLS,
		CONTROLSXBOX,
		CONTROLSKEYBOARD
	};

	private Menus menus;

    public Texture2D logo;

	public Texture2D controlsKeyboard;
	public Texture2D controlsXbox;

    public GUISkin skin;
	public GUISkin Label;

    private Menus GUIMenu = Menus.MAIN_MENU;

    public float musicValue;
    public float soundValue;

    public GameObject musicManager;
    public GameObject soundManager;

	bool[] mainMenuSelections = new bool[4] {false, false,  false, false};
	bool[] controlMenuSelections = new bool[3] {false, false, false};
	bool[] optionMenuSelections = new bool[3] {false, false, false};
	private bool backButton;
	private bool backButtonControls;
	private bool keyPressed;

	string[] mainMenuTexts = new string[4] { "stArt", "controls", "credits", "exit" };
	string[] controlMenuTexts = new string[3] {"xbox", "keyboard", "BAck"};
	string[] optionMenuTexts = new string[3] { "BAck", "Music", "Sound"};
	string[] miscTexts = new string[1] { "BAck"};

	private Dictionary<Menus, string[]> currentMenu = new Dictionary<Menus, string[]>(); 

	private int selected = 0;

	private MenuSounds menuSounds;

    void Start()
    {
		currentMenu.Add(Menus.MAIN_MENU, mainMenuTexts);
		//currentMenu.Add(Menus.OPTIONS, optionMenuTexts);
		currentMenu.Add(Menus.CREDITS, miscTexts);
		currentMenu.Add(Menus.CONTROLS, controlMenuTexts);
		currentMenu.Add(Menus.CONTROLSXBOX, miscTexts);
		currentMenu.Add(Menus.CONTROLSKEYBOARD, miscTexts);

        GUIMenu = Menus.MAIN_MENU;

        PlayerPrefs.SetFloat("MusicVolume", 1.00f);
        musicValue = PlayerPrefs.GetFloat("MusicVolume");

        PlayerPrefs.SetFloat("SoundVolume", 1.00f);
        soundValue = PlayerPrefs.GetFloat("SoundVolume");

		menuSounds = GameObject.Find("MainMenuSounds").GetComponent<MenuSounds>();
    }

	private int previousSelected;
	void Update()
	{
		selected = menuSelection(currentMenu[GUIMenu], selected);
		if (previousSelected != selected)
			navigationSound();
		previousSelected = selected;

		if (Input.GetButtonDown("Jump"))
			keyPressed = true;
	}

    void OnGUI()
    {
		GUI.skin = skin;
		//makes font for button scalable too
		GUI.skin.button.fontSize = (int)(23.0f * (float)(Screen.width)/1920.0f);
		GUI.skin.label.fontSize = (int)(25.0f * (float)(Screen.width)/1920.0f);

		GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

	    switch (GUIMenu)
	    {
		    case Menus.MAIN_MENU:
				MainMenuScreen();
			    break;
		    case Menus.OPTIONS:
				//OptionsScreen();
			    break;
		    case Menus.CREDITS:
				Credits();
			    break;
		    case Menus.CONTROLS:
				Controls();
			    break;
			case Menus.CONTROLSKEYBOARD:
				ControlsKeyboard();
				break;
			case Menus.CONTROLSXBOX:
				ControlsXbox();
				break;
		    default:
			    throw new ArgumentOutOfRangeException();
	    }

		GUI.EndGroup();
    }

    void MainMenuScreen()
    {
        GUI.Label(new Rect(Screen.width / 6, Screen.height / 15, Screen.width / 1.5f, Screen.height / 1.5f), logo);

	    startMultiFocusArea(mainMenuSelections, mainMenuTexts);

		SetMenuSelectionTrue(mainMenuSelections);

		if (mainMenuSelections[0])
		{
			selection();
			resetSelected();
			mainMenuSelections[0] = false;
			AutoFade.LoadLevel(2, 3, 1, Color.black);
		}

        if (mainMenuSelections[1])
        {
            selection();
            resetSelected();
            mainMenuSelections[1] = false;
            GUIMenu = Menus.CONTROLS;
        }

		if (mainMenuSelections[2])
		{
			selection();
			resetSelected();
			mainMenuSelections[2] = false;
			GUIMenu = Menus.CREDITS;
		}

		if (mainMenuSelections[3])
		{
			selection();
			Application.Quit();
		}

		endMultiFocusArea(mainMenuTexts);
    }

	private void selection()
	{
		if(!menuSounds.AudioSources[0].isPlaying)
			menuSounds.AudioSources[0].Play();
	}

	void startMultiFocusArea(bool[] menuSelection, string[] texts)
	{
		for (int i = 0; i < texts.Length; i++)
		{
			GUI.SetNextControlName(texts[i]);
			menuSelection[i] = GUI.Button(new Rect(Screen.width / 2.26f, (Screen.height / 2.2f) + (60 * i), Screen.width / 9, Screen.height / 13), texts[i]);
		}
	}

	void endMultiFocusArea(string[] texts)
	{
		GUI.FocusControl(texts[selected]);
	}

	private void resetSelected()
	{
		selected = 0;
	}

	private void SetMenuSelectionTrue(bool[] selection)
	{
		if (keyPressed)
		{
			selection[selected] = true;
			keyPressed = false;
		}
	}

	private void navigationSound()
	{
		if(!menuSounds.AudioSources[2].isPlaying)
			menuSounds.AudioSources[2].Play();
	}

	private void SetMenuSelectionTrue(ref bool selection)
	{
		if (keyPressed)
		{
			selection = true;
			keyPressed = false;
		}
	}

	private float timer;
	float input;
	private int menuSelection(string[] buttons, int selected)
	{
		bool inputCheck = ScrollUpThroughMenu();

		if (inputCheck)
			input = Input.GetAxis("Vertical");

		if (input < 0f)
		{
			if (selected == 0)
				selected = buttons.Length - 1;
			else
				selected -= 1;
		}
		else if (input > 0f)
		{
			if (selected == buttons.Length - 1)
				selected = 0;
			else
				selected += 1;
		}
			
		return selected;
	}

	private bool ScrollUpThroughMenu()
	{
		bool inputCheck = false;
		if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f &&
			(Input.GetAxis("Horizontal") < 0.1f && Input.GetAxis("Horizontal") > -0.1f))
		{
			if (timer > 0.5f)
			{
				timer = 0f;
			}

			if (Util.compareEachFloat(timer, 0.0f))
			{
				inputCheck = true;
			}
			else
			{
				input = 0f;
			}

			timer += Time.deltaTime;
		}
		
		if (Input.GetAxis("Vertical") < 0.1f && Input.GetAxis("Vertical") > -0.1f)
		{
			input = 0f;
			timer = 0f;
			inputCheck = false;
		}
		return inputCheck;
	}

    void OptionsScreen()
    {
		GUI.Label(new Rect(Screen.width / 6, Screen.height / 15, Screen.width / 1.5f, Screen.height / 1.5f), logo);

		startMultiFocusArea(optionMenuSelections, optionMenuTexts, Label);
		SetMenuSelectionTrue(optionMenuSelections);

		if(optionMenuSelections[0])
		{
			goBackSound();
			returnToMainMenu();
		}
		if(selected == 1)
			musicValue = AdjustSlider(musicValue);
		else if (selected == 2)
			soundValue = AdjustSlider(soundValue);

		exitCurrentMenu();
     
        musicValue = GUI.HorizontalSlider(new Rect(210, 235, 200, 20), musicValue, 0.0F, 1.00F);
        PlayerPrefs.SetFloat("MusicVolume", musicValue);

	
        //GUI.Label(new Rect(185, 210, 254, 47), "Music", skin.button);

		soundValue = GUI.HorizontalSlider(new Rect(210, 300, 200, 20), soundValue, 0.0F, 1.00F);
        PlayerPrefs.SetFloat("SoundVolume", soundValue);

        //GUI.Label(new Rect(185, 275, 254, 47), "Sound");

		endMultiFocusArea(optionMenuTexts);
    }

	void startMultiFocusArea(bool[] menuSelection, string[] texts, GUISkin skin)
	{
		for (int i = 0; i < texts.Length; i++)
		{
			GUI.SetNextControlName(texts[i]);
			if(i > 0)
				menuSelection[i] = GUI.Button(new Rect(Screen.width / 2.26f, (Screen.height / 2.2f) + (60 * i), Screen.width / 9, Screen.height / 13), texts[i], skin.label);
			else
				menuSelection[i] = GUI.Button(new Rect(Screen.width / 2.26f, (Screen.height / 2.2f) + (60 * i), Screen.width / 9, Screen.height / 13), texts[i]);
		}
	}

	private void exitCurrentMenu()
	{
		if (backButton)
		{
			goBackSound();
			resetSelected();
			resetBackButton();
			returnToMainMenu();
		}

		if (backButtonControls)
		{
			goBackSound();
			resetSelected();
			resetBackButton();
			returnToMainMenu();
		}
	}

	private void goBackSound()
	{
		if(!menuSounds.AudioSources[1].isPlaying)
			menuSounds.AudioSources[1].Play();
	}

	void startMultiFocusArea(string[] texts)
	{
		GUI.SetNextControlName(texts[0]);
		backButton = GUI.Button(new Rect(Screen.width / 2.26f, Screen.height / 1.3f, Screen.width / 9, Screen.height / 13), texts[0]);
	}

	void startMultiFocusAreaC(string[] texts)
	{
		GUI.SetNextControlName(texts[0]);
		backButtonControls = GUI.Button(new Rect(Screen.width / 2.26f, Screen.height / 1.2f, Screen.width / 9, Screen.height / 13), texts[0]);
	}

	private void returnToMainMenu()
	{
		GUIMenu = Menus.MAIN_MENU;
	}

	private void resetBackButton()
	{
		backButton = false;
		backButtonControls = false;
	}

	private float AdjustSlider(float valueToAdjust)
	{
		if (Input.GetAxis("Horizontal") < 0.0f || Input.GetAxis("Horizontal") > 0.0f)
			valueToAdjust += (Input.GetAxis("Horizontal") / 100f);

		if (valueToAdjust > 1f)
			valueToAdjust = 1f;
		else if (valueToAdjust < 0f)
			valueToAdjust = 0f;

		return valueToAdjust;
	}

	void Credits()
    {
		GUI.Label(new Rect(Screen.width / 6, Screen.height / 15, Screen.width / 1.5f, Screen.height / 1.5f), logo);

		GUI.Label(new Rect(Screen.width / 2.7f, Screen.height / 2.23f, Screen.width / 4, Screen.height / 25), " Alex Lueck - LeAd Design");
		GUI.Label(new Rect(Screen.width / 3.134f, Screen.height / 2.05f, Screen.width / 2.8f, Screen.height / 25), " MichAel Lojkovic - LeAd ProgrAmmer");
		GUI.Label(new Rect(Screen.width / 2.808f, Screen.height / 1.9f, Screen.width / 3.7f, Screen.height / 25), " Justin Terry - ProgrAmmer");
		GUI.Label(new Rect(Screen.width / 2.875f, Screen.height / 1.77f, Screen.width / 4.5f, Screen.height / 25), " PAtric HArmon - Artist");
		GUI.Label(new Rect(Screen.width / 3.265f, Screen.height / 1.65f, Screen.width / 3, Screen.height / 25), " Geoff Lightbourn - Sound LiAson");
		GUI.Label(new Rect(Screen.width / 2.77f, Screen.height / 1.55f, Screen.width / 4.2f, Screen.height / 25), " KAnoA Doblin - Composer");
		GUI.Label(new Rect(Screen.width / 3.23f, Screen.height / 1.46f, Screen.width / 3.2f, Screen.height / 25), " JessicA BorlovAn - UI Scripting");


		startMultiFocusArea(miscTexts);
		SetMenuSelectionTrue(ref backButton);

		exitCurrentMenu();

		endMultiFocusArea(miscTexts);
    }

	void Controls()
	{
		GUI.Label(new Rect(Screen.width / 6, Screen.height / 15, Screen.width / 1.5f, Screen.height / 1.5f), logo);

		startMultiFocusArea(controlMenuSelections, controlMenuTexts);
		SetMenuSelectionTrue(controlMenuSelections);

		if (controlMenuSelections[0])
		{
			selection();
			resetSelected();
			mainMenuSelections[0] = false;
			GUIMenu = Menus.CONTROLSXBOX;
		}
		
		if (controlMenuSelections[1])
		{
			selection();
			resetSelected();
			mainMenuSelections[1] = false;
			GUIMenu = Menus.CONTROLSKEYBOARD;
		}
		
		if (controlMenuSelections[2])
		{
			goBackSound();
			resetSelected();
			resetBackButton();
			returnToMainMenu();
		}

		endMultiFocusArea(controlMenuTexts);
	}

	void ControlsXbox()
	{
		GUI.Label(new Rect(Screen.width / 6, Screen.height / 15, Screen.width / 1.5f, Screen.height / 1.5f), logo);
		
		GUI.Label(new Rect(Screen.width / 6.2f, Screen.height / 2.4f, Screen.width / 1.5f, Screen.height / 2.5f), controlsXbox);

		startMultiFocusAreaC(miscTexts);
		SetMenuSelectionTrue(ref backButtonControls);

		exitCurrentMenu();
		
		endMultiFocusArea(miscTexts);
	}

	void ControlsKeyboard()
	{
		GUI.Label(new Rect(Screen.width / 6, Screen.height / 15, Screen.width / 1.5f, Screen.height / 1.5f), logo);
		
		GUI.Label(new Rect(Screen.width / 5.7f, Screen.height / 2.4f, Screen.width / 1.5f, Screen.height / 2.5f), controlsKeyboard);

		startMultiFocusAreaC(miscTexts);
		SetMenuSelectionTrue(ref backButtonControls);

		exitCurrentMenu();
		
		endMultiFocusArea(miscTexts);
	}
}
