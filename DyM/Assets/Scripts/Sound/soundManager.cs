using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	private float soundVolume;
	private float currentVolume;

	private MenuSounds menuSounds;

	void Start()
	{
		menuSounds = GameObject.Find("MainMenuSounds").GetComponent<MenuSounds>();
	}

	void Update () 
	{
		soundVolume = PlayerPrefs.GetFloat("SoundVolume");

		for (int i = 0; i < menuSounds.AudioSources.Count; i++) 
		{
			menuSounds.AudioSources[i].volume = soundVolume;
		}

	}
}
