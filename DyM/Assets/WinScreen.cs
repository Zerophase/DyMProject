using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour
{

    public Texture winScreenTexture;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),winScreenTexture);

        if (GUI.Button(new Rect(Screen.width/2, Screen.height/2, 150, 25), "Return to Menu"))
        {
            Application.LoadLevel("main_menu");
        }
    }
}
