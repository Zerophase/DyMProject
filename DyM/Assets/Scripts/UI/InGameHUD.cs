using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

public class InGameHUD : MonoBehaviour, IOwner
{
	[HideInInspector] 
	public static InGameHUD Instance;

	public Texture2D EmptyHealthBar;
	public Texture2D FullHealthBar;

	public GameObject Player;
	
	private int healthBarDisplay;	
	public Vector2 size = new Vector2(800, 40);
	public Vector2 pos = new Vector2(1400, 50);

	private Rect healthBar = new Rect(0f, 0f, 100, 35f);
	private Rect backgroundHealthBar;
	private Rect interfaceArea;

	[Inject]
	private IReceiver receiver;
	public IReceiver Receiver
	{
		set { receiver = value; }
	}

	void Start ()
	{
		Instance = this;
		receiver.Owner = this;
		receiver.SubScribe();

		backgroundHealthBar = new Rect(0f, 0f, 300f, size.y);
		interfaceArea = new Rect(50f, Screen.height / 2 - 200, size.x, size.y);
	}

	void OnGUI ()
	{	
		GUI.BeginGroup(interfaceArea);
			GUI.DrawTexture(backgroundHealthBar, EmptyHealthBar);
			GUI.DrawTexture(healthBar, FullHealthBar);
		GUI.EndGroup();
	}

	public void Receive(ITelegram telegram)
	{
		healthBarDisplay = (int) telegram.Message;
		healthBar.width = healthBarDisplay;
	}
}
