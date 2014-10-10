using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Text;

namespace Assets.Scripts.Utilities.Messaging.Interfaces
{
	public interface IOwner 
	{
		IReceiver Receiver { set; }
		void Receive(ITelegram telegram);
	}
}
