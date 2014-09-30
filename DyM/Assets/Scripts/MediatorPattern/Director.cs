using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public class Director : MonoBehaviour, IOwner
	{
		[Inject]
		private IReceiver receiver;

		

		private List<Mediator> mediatedObject = new List<Mediator>();  
		public IReceiver Receiver
		{
			set { receiver = value; }
		}

		public void Receive(ITelegram telegram)
		{
			if(telegram.Receiver == this)
				mediatedObject.Add((Mediator)telegram.Message);
		}

		void Awake()
		{
			this.receiver.Owner = this;
			receiver.SubScribe();
		}

		void Update()
		{
			Debug.Log("Amount of objects in mediated object: " +
			mediatedObject.Count);
		}
	}
}
