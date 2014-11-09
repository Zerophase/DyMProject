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
	public class ForkLightningGun : RangeWeaponBase
	{
		private int projectileCount = 0;

		[Inject]
		public ForkLightningGun(IReceiver receiver, IMessageDispatcher messageDispatcher, 
			IProjectile projectile, IBulletPool bulletPool) :
			base(receiver, messageDispatcher, projectile, bulletPool)
		{
			fireRate = 0.25f;
		}

		public override bool FireRate(float time)
		{
			bool keepFiring = true;
			
			if(projectileCount == 3)
			{
				
				keepFiring = base.FireRate(time);
				if (keepFiring)
					projectileCount = 0;
			}
			else
			{
				projectileCount++;
			}
			return keepFiring;
		}
	}
}
