using UnityEngine;
using System.Collections;

public class InGameHUD : MonoBehaviour {

	public Texture2D EmptyHealthBar;
	public Texture2D FullHealthBar;

	public GameObject Player;
	
	private int healthBarDisplay;	
	public Vector2 size = new Vector2(800, 40);
	public Vector2 pos = new Vector2(1400, 50);

	void Start ()  
	{
	
	}

	void OnGUI ()
	{	
		//Bar
		GUI.BeginGroup(new Rect(50, Screen.height / 2 - 200, size.x, size.y));
		
		GUI.Label(new Rect(0, 0, size.x, size.y), EmptyHealthBar);
		
			GUI.BeginGroup(new Rect(0,0, size.x, size.y));
			GUI.Label(new Rect(0, 0, size.x * healthBarDisplay, size.y), FullHealthBar);
			GUI.EndGroup();

		GUI.EndGroup();
		//End bar


		
	}

	void handMessage(ITelegram telegram)
	{

	}
}
