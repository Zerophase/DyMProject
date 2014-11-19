using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSounds : MonoBehaviour 
{
	private List<AudioSource> audioSources = new List<AudioSource>();
	public List<AudioSource> AudioSources { get { return audioSources;}}

	void Awake () 
	{
		var audioSource = GetComponents<AudioSource>();

		for (int i = 0; i < audioSource.Length; i++)
		{
			audioSources.Add(audioSource[i]);
		}
	}

	void Update () 
	{
	
	}
}
