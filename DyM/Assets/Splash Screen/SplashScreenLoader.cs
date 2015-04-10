using UnityEngine;
using System.Collections;

public class SplashScreenLoader : MonoBehaviour {

	public float delayTime = 5;
	public bool done = false;

	private float timer;

	// Use this for initialization
	void Start () {
		timer = delayTime;

		StartCoroutine("SomeFunction");
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		if (timer > 0)
			return;
		if (done)
			Application.LoadLevel(0);
	}

	IEnumerator SomeFunction() {

		yield return null;
	}

}
