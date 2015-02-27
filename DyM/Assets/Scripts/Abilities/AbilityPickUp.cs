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
using Assets.Scripts.GameObjects;

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

		public void PickUp(Player player)
		{

			ability.PlayerCharacter = player.character;
			messageDispatcher.DispatchMessage(new Telegram(player.character, ability));
		}
	}
}
