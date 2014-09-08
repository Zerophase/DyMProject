using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utilities.Messaging.Interfaces
{
	public interface IReceiver
	{
		Telegram TestTelegram { get; set; }
		IOwner Owner { get; set; }
		void SubScribe();
		void HandleMessage(Telegram telegram);
	}
}
