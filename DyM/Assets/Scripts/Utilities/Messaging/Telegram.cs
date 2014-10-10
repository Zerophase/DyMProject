using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.Utilities.Messaging.Interfaces;

namespace Assets.Scripts.Utilities.Messaging
{
	public class Telegram : ITelegram
	{
		private IOwner receiver;

		public IOwner Receiver
		{
			get { return receiver; }
		}

		private object message;

		public object Message
		{
			get { return message; }
		}
		public Telegram()
		{

		}

		public Telegram(IOwner receiver, object message)
		{
			this.receiver = receiver;
			this.message = message;
		}

		public Telegram(Director d, object message)
		{
			this.receiver = d;
			this.message = message;
		}
	}
}
