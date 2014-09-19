using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Utilities.Messaging.Interfaces;

namespace Assets.Scripts.Abilities
{
	public class Ability : AbilityBase
	{
		public Ability(IMessageDispatcher messageDispatcher)
			:base(messageDispatcher)
		{
			
		}
	}
}
