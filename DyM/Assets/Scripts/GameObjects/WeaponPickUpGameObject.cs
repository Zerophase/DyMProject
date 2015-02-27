using Assets.Scripts.DependencyInjection;
using Assets.Scripts.MediatorPattern;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using UnityEngine;

namespace Assets.Scripts.GameObjects
{
	public class WeaponPickUpGameObject : PhysicsMediator
	{
		public WeaponTypes WeaponTypes;
		[Inject]
		private WeaponPickUpFactory weaponPickUpFactory;
		private WeaponPickUp weaponPickUp;

		public void Start()
		{
			weaponPickUp = weaponPickUpFactory.Create(WeaponTypes);
			
			base.Start();
		}

		public void PickUp(GameObject player)
		{
			weaponPickUp.PickUp(player.GetComponent<Player>());
			Destroy(gameObject);
		}
	}
}
