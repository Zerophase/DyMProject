using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.UI;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour, IOwner
{
	[Inject]
	private IEntityManager entityManager;

	[Inject]
	private IReceiver receiver;
	public IReceiver Receiver
	{
		set { receiver = value; }
	}

	[Inject]
	private IIds id;

	private Slider healthSlider;
	private Slider timeSlider;
	void Start()
	{
		receiver.Owner = this;
		receiver.SubScribe();
		id.CreateId();

		entityManager.Add(Entities.HUD, id.ObjectId, this);

		var healthBar = GameObject.Find("HPBar");
		healthSlider = healthBar.GetComponent<Slider>();

		var timeBar = GameObject.Find("TPBar");
		timeSlider = timeBar.GetComponent<Slider>();
	}

	public void Receive(ITelegram telegram)
	{
		if (telegram.Message is HealthMessage)
		{
			healthSlider.normalizedValue = (telegram.Message as HealthMessage).Message / 300.0f;
		}
		else if (telegram.Message is AbilityMessage)
		{
			timeSlider.normalizedValue = (telegram.Message as AbilityMessage).Message / 5f;
		}
	}
}
