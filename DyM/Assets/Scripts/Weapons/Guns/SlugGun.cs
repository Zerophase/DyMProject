using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Utilities.Messaging.Interfaces;
using Assets.Scripts.Weapons.Bases;

namespace Assets.Scripts.Weapons.Guns
{
	public class SlugGun : RangeWeaponBase
	{
		public SlugGun(IReceiver receiver, IMessageDispatcher messageDispatcher, 
			IProjectile projectile, IBulletPool bulletPool) :
			base(receiver, messageDispatcher, projectile, bulletPool)
		{
			fireRate = 1f;
		}
	}
}