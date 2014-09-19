using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utilities.Messaging.Interfaces
{
	public delegate void SendMessageHandler<T>(T telegram);

	public interface IMessageDispatcher
	{
		event SendMessageHandler<ITelegram> SendMessage;
		void DispatchMessage(ITelegram telegram);
	}
}
