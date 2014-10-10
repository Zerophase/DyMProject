using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Character.Interfaces;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons.Interfaces;
using ModestTree.Zenject;
using Assets.Scripts.Weapons.Bases;
using UnityEngine;

namespace Assets.Scripts.Weapons
{


	public class TestRangeWeapon : RangeWeaponBase
	{
		public TestRangeWeapon() : base(0)
		{
		}

		public TestRangeWeapon(IProjectile projectile) : base(0, projectile)
		{
		}

		[Inject]
		public TestRangeWeapon(IReceiver receiver, IMessageDispatcher messageDispatcher,
			IBulletPool bulletPool = null) 
			: base(receiver, messageDispatcher, bulletPool)
		{
		}
	}

	public class TestWeapon2 : RangeWeaponBase
	{
		public TestWeapon2() : base(1)
		{
		}
	}

	public class TestWeapon3 : RangeWeaponBase
	{
		public TestWeapon3() : base(2)
		{
		}
	}
}
