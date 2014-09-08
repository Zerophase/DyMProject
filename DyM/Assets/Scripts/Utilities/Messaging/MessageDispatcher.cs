using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities.CustomExceptions;
using Assets.Scripts.Utilities.Messaging.Interfaces;

namespace Assets.Scripts.Utilities.Messaging
{
	public class MessageDispatcher : IMessageDispatcher
	{
		public event SendMessageHandler<Telegram> SendMessage;


		public void DispatchMessage(Telegram telegram)
		{
			if (SendMessage != null)
				SendMessage(telegram);
		}
	}
}
