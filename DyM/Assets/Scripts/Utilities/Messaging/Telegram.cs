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
			set { receiver = value; }
		}

		private object message;
		public object Message
		{
			get { return message; }
			set { message = value; }
		}

		private bool global;
		public bool Global
		{
			get { return global; }
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

		public Telegram(IOwner receiver, object message, bool global)
		{
			this.receiver = receiver;
			this.message = message;
			this.global = global;
		}
	}
}
