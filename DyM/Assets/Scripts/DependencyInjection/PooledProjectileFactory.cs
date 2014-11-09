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

		private ProjectileFactory projectileFactory;
		public PooledProjectileFactory(Instantiator instantiator)
		{
			this.instantiator = instantiator;
			projectileFactory = instantiator.Instantiate<ProjectileFactory>();
		}

		public IPooledProjectile Create(IProjectile projectile)
		{
			return instantiator.Instantiate<PooledProjectile>(
				projectileFactory.Create(projectile));
		}
	}
}
