using System.Collections;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public partial class InGameHUD : MonoBehaviour
	{
		[Inject]
		private IEntityManager entityManager;

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
	}

	public partial class InGameHUD : IOwner
	{

		[Inject]
		private IReceiver receiver;
		public IReceiver Receiver
		{
			set { receiver = value; }
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
}