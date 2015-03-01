using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.UI;

public class InGameHUD : MonoBehaviour, IOwner
{
    [Inject]
    private IEntityManager entityManager;

	public Texture2D EmptyHealthBar;
	public Texture2D FullHealthBar;
    public Texture2D EmptyTimeBar;
    public Texture2D FullTimeBar;
  
	public GameObject Player;
	
	private int healthBarDisplay;
    private int timeBarDisplay;

	private Rect healthBar = new Rect(5f, 0f, 100, 35f);
    private Rect timeBar = new Rect(5f, 40f, 0, 25f);
	private Rect backgroundHealthBar;
    private Rect backgroundTimeBar;
	private Rect interfaceArea;
	private Rect frame = new Rect(-5, -2, 250, 100);
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
		interfaceArea = new Rect(50f, Screen.height / 2 - 450, 400, 200);
	}

	void OnGUI ()
	{
        

		GUI.BeginGroup(interfaceArea);
			GUI.DrawTexture(backgroundHealthBar, EmptyHealthBar);
			GUI.DrawTexture(healthBar, FullHealthBar);

            GUI.DrawTexture(backgroundTimeBar, EmptyTimeBar);
            GUI.DrawTexture(timeBar, FullTimeBar);

		GUI.EndGroup();
	}

	public void Receive(ITelegram telegram)
	{
		if(telegram.Message is HealthMessage)
		{
			healthBarDisplay = (telegram.Message as HealthMessage).Message;
			healthBar.width = healthBarDisplay;
		}
		else if (telegram.Message is AbilityMessage)
		{
			timeBarDisplay = (int)(telegram.Message as AbilityMessage).Message;
			timeBar.width = timeBarDisplay * 60;
		}
	}
}
