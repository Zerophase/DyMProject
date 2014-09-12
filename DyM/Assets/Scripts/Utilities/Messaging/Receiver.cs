using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;
using ModestTree.Zenject;

namespace Assets.Scripts.Utilities.Messaging
{
	public class Receiver : IReceiver
	{
		private IMessageDispatcher messageDispatcher;
		private IOwner owner;
		public IOwner Owner { get { return owner; } set { owner = value; }}

		public Telegram TestTelegram { get; set; }

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

		public void HandleMessage(Telegram telegram)
		{
			TestTelegram = telegram;
			if (owner == telegram.Receiver)
				owner.Receive(telegram);

		}

		public void SubScribe()
		{
			messageDispatcher.SendMessage += HandleMessage;
		}
	}
}
