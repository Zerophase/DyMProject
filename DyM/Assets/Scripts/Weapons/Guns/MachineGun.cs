using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons.Bases;
using ModestTree.Zenject;

namespace Assets.Scripts.Weapons.Guns
{
	public class MachineGun : RangeWeaponBase
	{
		[Inject]
		public MachineGun(IReceiver receiver, IMessageDispatcher messageDispatcher, 
			IProjectile projectile, IBulletPool bulletPool = null) :
			base(receiver, messageDispatcher, projectile, bulletPool)
		{
			
		}
	}
}
