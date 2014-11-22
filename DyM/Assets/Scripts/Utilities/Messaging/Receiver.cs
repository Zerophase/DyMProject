using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.Utilities.Messaging
{
	public class Receiver : IReceiver
	{
		private IMessageDispatcher messageDispatcher;
		private IOwner owner;
		public IOwner Owner { get { return owner; } set { owner = value; }}

		public ITelegram TestTelegram { get; set; }

		[Inject]
		public Receiver(IMessageDispatcher messageDispatcher)
		{
			this.messageDispatcher = messageDispatcher;
		}

		
		public Receiver(IMessageDispatcher messageDispatcher, IOwner owner)
		{
			this.messageDispatcher = messageDispatcher;
			this.owner = owner;
		}

		public void HandleMessage(ITelegram telegram)
		{
			if (!telegram.Global && owner == telegram.Receiver)
				owner.Receive(telegram);
			else if (telegram.Global && owner.GetType() == telegram.Receiver.GetType())
				owner.Receive(telegram);
		}

		public void SubScribe()
		{
			messageDispatcher.SendMessage += HandleMessage;
		}
	}
}
