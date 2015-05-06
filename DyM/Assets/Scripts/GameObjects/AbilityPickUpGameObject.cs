using Assets.Scripts.Abilities;
using Assets.Scripts.Abilities.Interfaces;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.DependencyInjection;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.Utilities.Messaging;
using ModestTree.Zenject;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.GameObjects
{
	public class AbilityPickUpGameObject : ItemPickUp
	{
		public AbilityTypes abilityTypes;

		[Inject] private AbilityPickUpFactory abilityPickUpFactory;
		private AbilityPickUp abilityPickUp;

		public void Start()
		{
			abilityPickUp = abilityPickUpFactory.Create(abilityTypes);

			base.Start();
		}

		public override void PickUp(GameObject player)
		{
			abilityPickUp.PickUp(player.GetComponent<Player>());
			Destroy(gameObject);
		}
	}
}

