using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;

namespace Assets.Scripts.Abilities
{
	public class AbilityPickUp : IPickUp
	{
		[Inject] 
		private IMessageDispatcher messageDispatcher;
		
		private IAbility ability;


		public AbilityPickUp(IAbility ability)
		{
			this.ability = ability;
		}

		public void PickUp(ICharacter character)
		{
			messageDispatcher.DispatchMessage(new Telegram(character, ability));
		}
	}
}
