using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.Utilities.Messaging;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.GameObjects
{
	public class AbilityPickUp : PhysicsMediator
	{
		[Inject] 
		private IAbility ability;

		public void PickUp(GameObject player)
		{
			ability.PickUp(player.GetComponent<Player>().character);
			Destroy(gameObject);
		}
	}
}

