using UnityEngine;
using System.Collections;

public class musicManager : MonoBehaviour {

    private float musicVolume;
    public float currentVolume;
	
	void Update () 
    {
		musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        currentVolume = (musicVolume * 0.1f);
        this.gameObject.GetComponent<AudioSource>().volume = currentVolume;
	}
}
