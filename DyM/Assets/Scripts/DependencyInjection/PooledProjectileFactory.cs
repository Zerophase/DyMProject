using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Projectiles.Interfaces;
using Assets.Scripts.Projectiles.Projectiles;
using ModestTree.Zenject;

namespace Assets.Scripts.DependencyInjection
{
	public class PooledProjectileFactory
	{
		private IFactory<IPooledProjectile> factory;
		private Instantiator instantiator;

		public PooledProjectileFactory(Instantiator instantiator)
		{
			this.instantiator = instantiator;
		}

		public IPooledProjectile Create(IProjectile projectile)
		{
			return instantiator.Instantiate<PooledProjectile>(projectile);
		}
	}
}
