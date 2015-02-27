using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using Assets.Scripts.GameObjects;

namespace Assets.Scripts.Weapons
{
	public class WeaponPickUp : IPickUp
	{
		[Inject]
		private IMessageDispatcher messageDispatcher;
		private IWeapon weapon;

		public WeaponPickUp(IWeapon weapon)
		{
			this.weapon = weapon;
		}

		public void PickUp(Player player)
		{
			messageDispatcher.DispatchMessage(new Telegram(player.character, weapon));
		}
	}
}
