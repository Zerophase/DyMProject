using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.StatusEffects;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using ModestTree.Zenject;

namespace Assets.Scripts.Abilities
{
	public class AbilityTwo : AbilityBase
	{
		public AbilityTwo(IMessageDispatcher messageDispatcher)
			: base(messageDispatcher)
		{
			statusEffect = StatusEffect.TESTTWO;
		}
	}
}
