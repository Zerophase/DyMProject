using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utilities.Messaging.Interfaces
{
	public interface ITelegram 
	{
		IOwner Receiver { get; set; }
		object Message { get; set; }
		bool Global { get; }
	}
}
