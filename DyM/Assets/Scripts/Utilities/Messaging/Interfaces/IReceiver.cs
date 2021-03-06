﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utilities.Messaging.Interfaces
{
	public interface IReceiver
	{
		ITelegram TestTelegram { get; set; }
		IOwner Owner { get; set; }
		void SubScribe();
		void HandleMessage(ITelegram telegram);
	}
}
