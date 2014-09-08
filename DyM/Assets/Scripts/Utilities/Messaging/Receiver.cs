using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons;

namespace Assets.Scripts.Utilities.Messaging
{
	public class Receiver : IReceiver
	{
		private IMessageDispatcher messageDispatcher;
		private IOwner owner;
		public IOwner Owner { get { return owner; } set { owner = value; }}

		public Telegram TestTelegram { get; set; }

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
