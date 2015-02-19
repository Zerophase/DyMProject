using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utilities.Messaging;

public class InGameHUD : MonoBehaviour, IOwner
{
    [Inject]
    private IEntityManager entityManager;

	public Texture2D EmptyHealthBar;
	public Texture2D FullHealthBar;
    public Texture2D EmptyTimeBar;
    public Texture2D FullTimeBar;
    public Texture2D BarFrame;

	public GameObject Player;
	
	private int healthBarDisplay;
    private int timeBarDisplay;

	private Rect healthBar = new Rect(5f, 0f, 100, 35f);
    private Rect timeBar = new Rect(5f, 40f, 100, 25f);
	private Rect backgroundHealthBar;
    private Rect backgroundTimeBar;
	private Rect interfaceArea;

	[Inject]
	private IReceiver receiver;
	public IReceiver Receiver
	{
		set { receiver = value; }
	}

    [Inject]
    private IIds id;

	void Start ()
	{
		receiver.Owner = this;
		receiver.SubScribe();
        id.CreateId();

        entityManager.Add(Entities.HUD, id.ObjectId, this);

		backgroundHealthBar = new Rect(5f, 0f, 300f, 35);
        backgroundTimeBar = new Rect(5f, 40f, 300f, 25);
		interfaceArea = new Rect(50f, Screen.height / 2 - 200, 400, 200);
	}

	void OnGUI ()
	{
        

		GUI.BeginGroup(interfaceArea);
			GUI.DrawTexture(backgroundHealthBar, EmptyHealthBar);
			GUI.DrawTexture(healthBar, FullHealthBar);

            GUI.DrawTexture(backgroundTimeBar, EmptyTimeBar);
            GUI.DrawTexture(timeBar, FullTimeBar);

            GUI.DrawTexture(new Rect(-5, -2, 250, 100), BarFrame);

		GUI.EndGroup();
	}

	public void Receive(ITelegram telegram)
	{
		healthBarDisplay = (int) telegram.Message;
		healthBar.width = healthBarDisplay;

        timeBarDisplay = (int)telegram.Message;
        timeBar.width = timeBarDisplay;
	}
}
