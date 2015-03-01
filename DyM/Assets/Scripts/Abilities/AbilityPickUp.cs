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
using Assets.Scripts.UI;

namespace Assets.Scripts.Abilities
{
	public class AbilityPickUp : IPickUp
	{
		[Inject] 
		private IMessageDispatcher messageDispatcher;
		
		private IAbility ability;

		[Inject]
		IEntityManager entityManager;

		public AbilityPickUp(IAbility ability)
		{
			this.ability = ability;
		}

		public void PickUp(Player player)
		{

			ability.PlayerCharacter = player.character;
			AbilityMessage message = new AbilityMessage(5f);
			this.messageDispatcher.DispatchMessage(new Telegram(entityManager.GetEntityFromID(Entities.HUD, 1), message));
			messageDispatcher.DispatchMessage(new Telegram(player.character, ability));
		}
	}
}
