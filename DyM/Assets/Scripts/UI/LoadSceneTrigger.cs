using UnityEngine;
using System.Collections;

public class LoadSceneTrigger : MonoBehaviour {

	private GameObject lastParagraph;

	void Start()
	{
		lastParagraph = GameObject.FindGameObjectWithTag ("loadScene");
	}

	void OnTriggerEnter(Collider lastParagraph) 
	{
			AutoFade.LoadLevel (3, 3, 1, Color.black);
	}
}
