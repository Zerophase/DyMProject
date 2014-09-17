using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities.CustomExceptions;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;

namespace Assets.Scripts.Utilities.Messaging
{
	public class MessageDispatcher : IMessageDispatcher
	{
		[Inject]
		public MessageDispatcher()
		{
			
		}

		public event SendMessageHandler<Telegram> SendMessage;


		public void DispatchMessage(Telegram telegram)
		{
			if (SendMessage != null)
				SendMessage(telegram);
		}
	}
}
