using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.MediatorPattern
{
	public abstract class Director : MonoBehaviour, IOwner
	{
		[Inject]
		protected IReceiver receiver;
		  
		public IReceiver Receiver
		{
			set { receiver = value; }
		}

		public virtual void Receive(ITelegram telegram)
		{
			throw new NotImplementedException();
		}

		void Awake()
		{
			receiver.Owner = this;
			receiver.SubScribe();
		}
	}
}
