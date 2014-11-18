﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utilities.Messaging.Interfaces
{
	public interface ITelegram 
	{
		IOwner Receiver { get; }
		object Message { get; }
		bool Global { get; }
	}
}
