using UnityEngine;
using System.Collections;

public class soundManager : MonoBehaviour {

	private float soundVolume;
	public float currentVolume;
	
	void Update () 
	{
		soundVolume = PlayerPrefs.GetFloat("SoundVolume");
		currentVolume = (soundVolume * 0.1f);
		this.gameObject.GetComponent<AudioSource>().volume = currentVolume;
	}
}
