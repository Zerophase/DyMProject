using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utilities.Messaging.Interfaces
{
	public interface IOwner
	{
		void Receive(Telegram telegram);
	}
}
