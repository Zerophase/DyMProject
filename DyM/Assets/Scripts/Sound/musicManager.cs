using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    private float musicVolume;
    public float CurrentVolume;
	
	void Update () 
    {
		musicVolume = PlayerPrefs.GetFloat("MusicVolume");
		gameObject.GetComponent<AudioSource>().volume = musicVolume;
	}
}
